using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a User in the system.
    /// Users belong to an Account and can have roles: Admin, Manager, or Employee.
    /// Supports email/mobile verification and hierarchical management structure.
    /// </summary>
    [Table("User")]
    public class User
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key to the Account this user belongs to
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Role of the user: Admin, Manager, or Employee
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserRole { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the user
        /// </summary>
        [Required]
        [StringLength(150)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user (unique)
        /// </summary>
        [StringLength(256)]
        public string? UserEmail { get; set; }

        /// <summary>
        /// Mobile phone number (unique)
        /// </summary>
        [StringLength(20)]
        public string? UserMobile { get; set; }

        /// <summary>
        /// Hashed password (bcrypt/Argon2 hash)
        /// </summary>
        [Required]
        [MaxLength(64)]
        public byte[] UserPasswordHash { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Indicates if email has been verified
        /// </summary>
        [Required]
        public bool EmailVerified { get; set; } = false;

        /// <summary>
        /// Indicates if mobile has been verified
        /// </summary>
        [Required]
        public bool MobileVerified { get; set; } = false;

        /// <summary>
        /// Payment type for workers: Fix Pay, FP + Incentive, or Incentive
        /// </summary>
        [StringLength(30)]
        public string? WorkerPaymentType { get; set; }

        /// <summary>
        /// Foreign key to the manager (self-referencing for hierarchy)
        /// </summary>
        public Guid? ManagerId { get; set; }

        /// <summary>
        /// User ID who created this user
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when user was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when user was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// User account status
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation properties
        /// <summary>
        /// The account this user belongs to
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;

        /// <summary>
        /// The manager of this user (for hierarchical structure)
        /// </summary>
        [ForeignKey("ManagerId")]
        public virtual User? Manager { get; set; }

        /// <summary>
        /// Collection of users managed by this user
        /// </summary>
        [InverseProperty("Manager")]
        public virtual ICollection<User> ManagedUsers { get; set; } = new List<User>();
    }
}
