using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TheBeautyHubAPI.Auth
{
    public static class JwtTokenValidation
    {
        public static Task OnMessageReceived(MessageReceivedContext context)
        {
            var queryToken = context.Request.Query["access_token"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(queryToken))
            {
                context.Token = queryToken.Trim();
                context.HttpContext.Items[AuthCenterClaimTypes.RawTokenItemKey] = context.Token;
                return Task.CompletedTask;
            }

            var header = context.Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(header))
                return Task.CompletedTask;

            var token = header.Trim();
            const string bearerPrefix = "Bearer ";
            while (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                token = token[bearerPrefix.Length..].Trim();

            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Token = token;
                context.HttpContext.Items[AuthCenterClaimTypes.RawTokenItemKey] = token;
            }

            return Task.CompletedTask;
        }

        public static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            if (AllowsAnonymous(context.HttpContext))
                context.NoResult();

            return Task.CompletedTask;
        }

        public static async Task OnTokenValidated(TokenValidatedContext context)
        {
            var principal = context.Principal;
            if (principal == null)
            {
                context.Fail("Invalid token.");
                return;
            }

            var userId = principal.FindFirst(AuthCenterClaimTypes.UserId)?.Value;
            var accountId = principal.FindFirst(AuthCenterClaimTypes.AccountId)?.Value;
            var sessionId = principal.FindFirst(AuthCenterClaimTypes.SessionId)?.Value;

            if (!Guid.TryParse(userId, out _) || !Guid.TryParse(accountId, out _) || !Guid.TryParse(sessionId, out _))
            {
                context.Fail("Invalid token.");
                return;
            }

            var options = context.HttpContext.RequestServices.GetRequiredService<Microsoft.Extensions.Options.IOptions<AuthCenterOptions>>().Value;
            if (!options.ValidateWithAuthCenter)
                return;

            var rawToken = context.HttpContext.Items[AuthCenterClaimTypes.RawTokenItemKey] as string;
            if (string.IsNullOrWhiteSpace(rawToken))
            {
                context.Fail("Invalid token.");
                return;
            }

            var validator = context.HttpContext.RequestServices.GetRequiredService<IAuthCenterTokenValidator>();
            var result = await validator.ValidateAsync(rawToken, context.HttpContext.RequestAborted);
            if (!result.IsValid)
            {
                context.Fail(result.Error ?? "Token is not valid for The Beauty Hub.");
                return;
            }

            if (principal.Identity is ClaimsIdentity identity)
            {
                foreach (var role in result.Roles)
                {
                    if (!identity.HasClaim(AuthCenterClaimTypes.Role, role))
                        identity.AddClaim(new Claim(AuthCenterClaimTypes.Role, role));
                }

                foreach (var permission in result.Permissions)
                {
                    if (!identity.HasClaim(AuthCenterClaimTypes.Permission, permission))
                        identity.AddClaim(new Claim(AuthCenterClaimTypes.Permission, permission));
                }
            }
        }

        public static async Task OnChallenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var message = context.AuthenticateFailure?.Message;
            if (string.IsNullOrWhiteSpace(message))
            {
                var hasAuthorization = context.Request.Headers.ContainsKey("Authorization");
                message = hasAuthorization
                    ? "Invalid token."
                    : "Missing Authorization header. Send the AuthCenter access token as Bearer.";
            }

            var payload = JsonSerializer.Serialize(new
            {
                status = false,
                message
            });

            await context.Response.WriteAsync(payload);
        }

        public static async Task OnForbidden(ForbiddenContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new
            {
                status = false,
                message = "You are not authorized to access this resource."
            });
            await context.Response.WriteAsync(payload);
        }

        private static bool AllowsAnonymous(HttpContext http)
        {
            if (http.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
                return true;

            var path = http.Request.Path.Value ?? string.Empty;
            return path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/health", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/uploads", StringComparison.OrdinalIgnoreCase)
                || path.Equals("/api/token/validate", StringComparison.OrdinalIgnoreCase);
        }
    }
}
