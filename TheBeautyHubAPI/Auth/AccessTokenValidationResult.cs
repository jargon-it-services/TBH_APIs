using System;
using System.Collections.Generic;

namespace TheBeautyHubAPI.Auth
{
    public class AccessTokenValidationResult
    {
        public bool IsValid { get; set; }
        public Guid? UserId { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? ApplicationId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public string? Message { get; set; }
    }
}
