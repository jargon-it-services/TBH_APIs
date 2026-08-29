using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a partner (customer/vendor) in the system.
    /// Stores partner information including contact details.
    /// </summary>
    [Table("Partner")]
    public class Partner
    {
        /// <summary>
        /// Unique identifier for the partner
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PartnerId { get; set; }

        /// <summary>
        /// Name of the partner
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of partner (e.g., Customer, Vendor)
        /// </summary>
        [StringLength(50)]
        public string? Type { get; set; }

        /// <summary>
        /// Partner's address
        /// </summary>
        [StringLength(500)]
        public string? Address { get; set; }

        /// <summary>
        /// Partner's mobile number (unique)
        /// </summary>
        [StringLength(20)]
        public string? Mobile { get; set; }

        /// <summary>
        /// Partner's email address (unique)
        /// </summary>
        [StringLength(256)]
        public string? Email { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? DateofBirth { get; set; }

        /// <summary>
        /// Gender: Male, Female, or Other
        /// </summary>
        [StringLength(20)]
        public string? Gender { get; set; }

        /// <summary>
        /// Foreign key to the Account
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Date and time when partner was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
