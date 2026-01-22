using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a report definition in the system.
    /// Defines report templates that can be generated for accounts.
    /// </summary>
    [Table("Reports")]
    public class Report
    {
        /// <summary>
        /// Unique identifier for the report
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ReportId { get; set; }

        /// <summary>
        /// Name of the report
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ReportName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if report is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when report was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when report was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        // Navigation properties
        /// <summary>
        /// Collection of report instances for accounts
        /// </summary>
        public virtual ICollection<ReportForAccount> ReportsForAccounts { get; set; } = new List<ReportForAccount>();
    }
}
