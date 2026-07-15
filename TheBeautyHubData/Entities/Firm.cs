using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a Firm (beauty salon/spa) in the system.
    /// Each firm belongs to an Account and contains business information.
    /// </summary>
    [Table("Firm")]
    public class Firm
    {
        /// <summary>
        /// Unique identifier for the firm
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FirmId { get; set; }

        /// <summary>
        /// Foreign key to the Account this firm belongs to
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Name of the firm/salon
        /// </summary>
        [Required]
        [StringLength(200)]
        public string FirmName { get; set; } = string.Empty;

        /// <summary>
        /// Physical address of the firm
        /// </summary>
        [StringLength(500)]
        public string? FirmAddress { get; set; }

        /// <summary>
        /// GSTIN (GST Identification Number) for tax purposes
        /// </summary>
        [StringLength(15)]
        public string? FirmGstin { get; set; }

        /// <summary>
        /// Primary contact person name
        /// </summary>
        [StringLength(20)]
        public string? FirmContact { get; set; }

        /// <summary>
        /// Contact email address
        /// </summary>
        [StringLength(256)]
        public string? FirmEmail { get; set; }

        /// <summary>
        /// Photo/logo URL or path
        /// </summary>
        [StringLength(500)]
        public string? FirmPhoto { get; set; }

        /// <summary>
        /// Owner's name
        /// </summary>
        [StringLength(150)]
        public string? FirmOwnerName { get; set; }

        /// <summary>
        /// Type of firm (e.g., Salon, Spa, Clinic)
        /// </summary>
        [StringLength(50)]
        public string? FirmType { get; set; }

        /// <summary>
        /// Business registration number
        /// </summary>
        [StringLength(100)]
        public string? FirmRegistration { get; set; }

        /// <summary>
        /// Logo URL or path
        /// </summary>
        [StringLength(500)]
        public string? FirmLogo { get; set; }

        /// <summary>
        /// User ID who created this firm
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when firm was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when firm was last updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        /// <summary>
        /// The account this firm belongs to
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;

        /// <summary>
        /// Collection of firm details associated with this firm
        /// </summary>
        public virtual ICollection<FirmDetails> FirmDetails { get; set; } = new List<FirmDetails>();
    }
}
