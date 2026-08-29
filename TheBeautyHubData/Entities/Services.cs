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

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        [StringLength(20)]
        public string ApplicableGender { get; set; } = "unisex";

        [Required]
        [Column("Type")]
        [StringLength(30)]
        public string OfferingType { get; set; } = "in_salon";

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaterialCost { get; set; }

        [Required]
        [StringLength(20)]
        public string CommissionType { get; set; } = "flat";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OtherCost { get; set; }

        [Required]
        public bool HomeServiceAvailable { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HomeVisitCharges { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ServiceRadiusKm { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExtraChargePerKm { get; set; }

        [Required]
        public bool AllBranches { get; set; } = true;

        [StringLength(500)]
        public string? Photo { get; set; }

        /// <summary>
        /// User ID who created this service
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when service was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when service was last updated
        /// </summary>
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

        public virtual ICollection<BranchService> BranchServices { get; set; } = new List<BranchService>();
    }
}
