using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a report instance generated for a specific account.
    /// Links reports to accounts with activation status.
    /// </summary>
    [Table("ReportsForAccount")]
    public class ReportForAccount
    {
        /// <summary>
        /// Unique identifier for the report-account link
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key to the Report
        /// </summary>
        [Required]
        public Guid ReportId { get; set; }

        /// <summary>
        /// Foreign key to the Account
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Indicates if this report is active for the account
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when report was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User ID who created this report link
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when report was last updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        // Navigation properties
        /// <summary>
        /// The report definition
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; } = null!;

        /// <summary>
        /// The account for which this report is generated
        /// </summary>
    }
}
