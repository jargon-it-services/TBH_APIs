using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents an Account in the system.
    /// Each account can be of type FirmOwner or Customer and supports subscription or one-time payment modes.
    /// Includes trial period tracking for new accounts.
    /// </summary>
    [Table("Account")]
    public class Account
    {
        /// <summary>
        /// Unique identifier for the account
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Unique account code (6-digit or longer identifier)
        /// </summary>
        [Required]
        [StringLength(12)]
        public string AccountCode { get; set; } = string.Empty;

        /// <summary>
        /// Name of the account
        /// </summary>
        [Required]
        [StringLength(200)]
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Type of account: FirmOwner or Customer
        /// </summary>
        [Required]
        [StringLength(20)]
        public string AccountType { get; set; } = string.Empty;

        /// <summary>
        /// Payment mode: subscription or one_time
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if account is currently in trial period
        /// </summary>
        [Required]
        public bool IsUnderTrial { get; set; } = false;

        /// <summary>
        /// Date and time when trial period started
        /// </summary>
        public DateTime? TrialStartedOn { get; set; }

        /// <summary>
        /// Duration of trial period in days
        /// </summary>
        public int? TrialDuration { get; set; }

        /// <summary>
        /// Date and time when trial period expires
        /// </summary>
        public DateTime? TrialExpiredOn { get; set; }

        /// <summary>
        /// User ID who created this account
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when account was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when account was last updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        // Navigation property
        /// <summary>
        /// Collection of users associated with this account
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
