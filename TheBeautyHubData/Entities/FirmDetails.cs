using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents detailed information about a firm.
    /// Links users to firms with account relationships for detailed tracking.
    /// </summary>
    [Table("FirmDetails")]
    public class FirmDetails
    {
        /// <summary>
        /// Unique identifier for the firm details record
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FirmDetailsId { get; set; }

        /// <summary>
        /// Foreign key to the User
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key to the Account
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Foreign key to the Firm
        /// </summary>
        [Required]
        public Guid FirmId { get; set; }

        /// <summary>
        /// Date and time when this record was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("FirmId")]
        public virtual Firm Firm { get; set; } = null!;
    }
}
