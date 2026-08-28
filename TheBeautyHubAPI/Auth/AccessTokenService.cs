using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TheBeautyHubCore.Constants;

namespace TheBeautyHubAPI.Auth
{
    public sealed class AccessTokenService : IAccessTokenService
    {
        private readonly TokenValidationParameters _validationParameters;
        private readonly AuthCenterOptions _authCenterOptions;
        private readonly IAuthCenterTokenValidator _authCenterTokenValidator;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public AccessTokenService(
            IOptions<JwtSettings> jwtOptions,
            IOptions<AuthCenterOptions> authCenterOptions,
            IAuthCenterTokenValidator authCenterTokenValidator)
        {
            var jwtSettings = jwtOptions.Value;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            {
                KeyId = AuthCenterClaimTypes.SigningKeyId
            };

            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = signingKey,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                TryAllIssuerSigningKeys = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                IssuerSigningKeyResolver = (_, _, _, _) => new[] { signingKey }
            };

            _authCenterOptions = authCenterOptions.Value;
            _authCenterTokenValidator = authCenterTokenValidator;
        }

        public string? ReadTokenFromRequest(HttpRequest request)
        {
            var queryToken = request.Query["access_token"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(queryToken))
                return queryToken.Trim();

            var header = request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(header))
                return null;

            var token = header.Trim();
            const string bearerPrefix = "Bearer ";
            while (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                token = token[bearerPrefix.Length..].Trim();

            return string.IsNullOrWhiteSpace(token) ? null : token;
        }

        public async Task<AccessTokenValidationResult> ValidateAsync(string? accessToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Invalid(ApiMessages.Common.MissingToken);
            }

            ClaimsPrincipal principal;
            try
            {
                principal = _tokenHandler.ValidateToken(accessToken, _validationParameters, out _);
            }
            catch (Exception)
            {
                return Invalid(ApiMessages.Common.InvalidToken);
            }

            var userId = ParseGuid(principal, AuthCenterClaimTypes.UserId);
            var accountId = ParseGuid(principal, AuthCenterClaimTypes.AccountId);
            var sessionId = ParseGuid(principal, AuthCenterClaimTypes.SessionId);
            if (!userId.HasValue || !accountId.HasValue || !sessionId.HasValue)
                return Invalid(ApiMessages.Common.InvalidToken);

            var roles = principal.FindAll(AuthCenterClaimTypes.Role).Select(c => c.Value).ToList();
            var permissions = principal.FindAll(AuthCenterClaimTypes.Permission).Select(c => c.Value).ToList();

            if (_authCenterOptions.ValidateWithAuthCenter)
            {
                var authCenter = await _authCenterTokenValidator.ValidateAsync(accessToken, cancellationToken);
                if (!authCenter.IsValid)
                    return Invalid(authCenter.Error ?? ApiMessages.Common.TokenNotValidForApp);

                if (authCenter.Roles.Count > 0)
                    roles = authCenter.Roles;
                if (authCenter.Permissions.Count > 0)
                    permissions = authCenter.Permissions;
            }

            return new AccessTokenValidationResult
            {
                IsValid = true,
                UserId = userId,
                AccountId = accountId,
                SessionId = sessionId,
                ApplicationId = ParseGuid(principal, AuthCenterClaimTypes.ApplicationId),
                Email = principal.FindFirst(AuthCenterClaimTypes.Email)?.Value
                    ?? principal.FindFirst(ClaimTypes.Email)?.Value,
                Name = principal.FindFirst(AuthCenterClaimTypes.Name)?.Value,
                Roles = roles.Distinct(StringComparer.OrdinalIgnoreCase).ToList(),
                Permissions = permissions.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
            };
        }

        private static AccessTokenValidationResult Invalid(string message)
        {
            return new AccessTokenValidationResult
            {
                IsValid = false,
                Message = message
            };
        }

        private static Guid? ParseGuid(ClaimsPrincipal principal, string claimType)
        {
            var value = principal.FindFirst(claimType)?.Value;
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}
