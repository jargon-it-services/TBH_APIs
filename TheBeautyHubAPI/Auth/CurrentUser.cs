using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TheBeautyHubAPI.Auth
{
    public sealed class CurrentUser : ICurrentUser
    {
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            var principal = httpContextAccessor.HttpContext?.User;
            if (principal?.Identity?.IsAuthenticated != true)
                return;

            UserId = GetGuid(principal, AuthCenterClaimTypes.UserId) ?? Guid.Empty;
            AccountId = GetGuid(principal, AuthCenterClaimTypes.AccountId) ?? Guid.Empty;
            SessionId = GetGuid(principal, AuthCenterClaimTypes.SessionId) ?? Guid.Empty;
            ApplicationId = GetGuid(principal, AuthCenterClaimTypes.ApplicationId);
            Email = principal.FindFirst(AuthCenterClaimTypes.Email)?.Value
                ?? principal.FindFirst(ClaimTypes.Email)?.Value
                ?? string.Empty;
            Roles = principal.FindAll(AuthCenterClaimTypes.Role)
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            Permissions = principal.FindAll(AuthCenterClaimTypes.Permission)
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public bool IsAuthenticated => UserId != Guid.Empty && AccountId != Guid.Empty;

        public Guid UserId { get; }

        public Guid AccountId { get; }

        public Guid SessionId { get; }

        public Guid? ApplicationId { get; }

        public string Email { get; } = string.Empty;

        public IReadOnlyList<string> Roles { get; } = Array.Empty<string>();

        public IReadOnlyList<string> Permissions { get; } = Array.Empty<string>();

        private static Guid? GetGuid(ClaimsPrincipal principal, string claimType)
        {
            var value = principal.FindFirst(claimType)?.Value;
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}
