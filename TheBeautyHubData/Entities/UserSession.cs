using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a user session for authentication and tracking.
    /// Stores JWT tokens, device info, and session metadata.
    /// </summary>
    [Table("UserSessions")]
    public class UserSession
    {
        /// <summary>
        /// Unique identifier for the session
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SessionId { get; set; }

        /// <summary>
        /// Foreign key to the User
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Date and time when session was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when session was last seen
        /// </summary>
        public DateTime? LastSeenAt { get; set; }

        /// <summary>
        /// IP address of the client (IPv4/IPv6)
        /// </summary>
        [StringLength(45)]
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent string from the client
        /// </summary>
        [StringLength(256)]
        public string? UserAgent { get; set; }

        /// <summary>
        /// Device identifier
        /// </summary>
        [StringLength(128)]
        public string? DeviceId { get; set; }

        /// <summary>
        /// JWT access token identifier
        /// </summary>
        [Required]
        public Guid AccessTokenJti { get; set; }

        /// <summary>
        /// Refresh token hash (SHA-256 of opaque token)
        /// </summary>
        [Required]
        [MaxLength(32)]
        public byte[] RefreshTokenHash { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Date and time when refresh token expires
        /// </summary>
        [Required]
        public DateTime RefreshTokenExpiresAt { get; set; }

        /// <summary>
        /// Date and time when session was revoked
        /// </summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// Reason for revocation
        /// </summary>
        [StringLength(200)]
        public string? RevocationReason { get; set; }

        // Navigation properties
        /// <summary>
        /// The user associated with this session
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
