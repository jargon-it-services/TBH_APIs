using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateUserSessionRequest
    {
        [Required]
        public Guid UserId { get; set; }
        
        [StringLength(45)]
        public string? IpAddress { get; set; }
        
        [StringLength(256)]
        public string? UserAgent { get; set; }
        
        [StringLength(128)]
        public string? DeviceId { get; set; }
        
        [Required]
        public Guid AccessTokenJti { get; set; }
        
        [Required]
        public byte[] RefreshTokenHash { get; set; } = Array.Empty<byte>();
        
        [Required]
        public DateTime RefreshTokenExpiresAt { get; set; }
    }

    public class UpdateUserSessionRequest
    {
        public DateTime? LastSeenAt { get; set; }
    }

    public class UserSessionResponse
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceId { get; set; }
        public Guid AccessTokenJti { get; set; }
        public byte[] RefreshTokenHash { get; set; } = Array.Empty<byte>();
        public DateTime RefreshTokenExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevocationReason { get; set; }
    }
}
