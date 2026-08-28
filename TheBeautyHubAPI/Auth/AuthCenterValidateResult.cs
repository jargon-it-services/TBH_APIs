using System;
using System.Collections.Generic;

namespace TheBeautyHubAPI.Auth
{
    public class AuthCenterValidateResult
    {
        public bool IsValid { get; set; }
        public Guid? UserId { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public string? Error { get; set; }
    }
}
