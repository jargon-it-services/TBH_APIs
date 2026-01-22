namespace TheBeautyHubCore.DTOs
{
    public class UserSessionDto
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

    public class CreateUserSessionDto
    {
        public Guid UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceId { get; set; }
        public Guid AccessTokenJti { get; set; }
        public byte[] RefreshTokenHash { get; set; } = Array.Empty<byte>();
        public DateTime RefreshTokenExpiresAt { get; set; }
    }

    public class UpdateUserSessionDto
    {
        public DateTime? LastSeenAt { get; set; }
    }
}
