using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a Service offered by a beauty salon/firm.
    /// Services can have incentive-based pricing for employees.
    /// </summary>
    [Table("Services")]
    public class Services
    {
        /// <summary>
        /// Unique identifier for the service
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Name of the service
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the service
        /// </summary>
        [StringLength(1000)]
        public string? ServiceDescription { get; set; }

        /// <summary>
        /// Price of the service
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServicePrice { get; set; } = 0;

        /// <summary>
        /// Foreign key to the Transaction Type (for service type categorization)
        /// </summary>
        public Guid? ServiceTypeId { get; set; }

        /// <summary>
        /// Foreign key to the Account
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Foreign key to the Firm (optional)
        /// </summary>
        public Guid? FirmId { get; set; }

        /// <summary>
        /// Indicates if incentive applies to this service
        /// </summary>
        [Required]
        public bool IsIncentiveApplicable { get; set; } = false;

        /// <summary>
        /// Incentive amount for employees
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? IncentiveAmount { get; set; }

        /// <summary>
        /// Incentive percentage for employees (0-100)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? IncentivePercentage { get; set; }

        /// <summary>
        /// User ID who created this service
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when service was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when service was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        /// <summary>
        /// The account this service belongs to
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;

        /// <summary>
        /// The firm this service belongs to (optional)
        /// </summary>
        [ForeignKey("FirmId")]
        public virtual Firm? Firm { get; set; }

        /// <summary>
        /// The transaction type for this service (optional)
        /// </summary>
        [ForeignKey("ServiceTypeId")]
        public virtual TransactionType? ServiceType { get; set; }
    }
}
