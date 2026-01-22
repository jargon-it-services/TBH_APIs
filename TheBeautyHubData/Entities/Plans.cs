using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a subscription plan available in the system.
    /// Plans define the pricing and features for beauty hub services.
    /// </summary>
    [Table("Plans")]
    public class Plans
    {
        /// <summary>
        /// Unique identifier for the plan
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PlanId { get; set; }

        /// <summary>
        /// Name of the plan (e.g., Basic, Premium, Enterprise)
        /// </summary>
        [Required]
        [StringLength(200)]
        public string PlanName { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the plan features
        /// </summary>
        [StringLength(1000)]
        public string? PlanDescription { get; set; }

        /// <summary>
        /// Cost of the plan
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PlanCost { get; set; }

        /// <summary>
        /// Date and time when plan was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if the plan is currently active and available for subscription
        /// </summary>
        [Required]
        public bool IsPlanActive { get; set; } = true;

        /// <summary>
        /// Date and time when plan was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Applicable to specific services or general (e.g., spa, salon)
        /// </summary>
        [StringLength(200)]
        public string? PlanAppliedTo { get; set; }

        // Navigation properties
        /// <summary>
        /// Collection of subscriptions using this plan
        /// </summary>
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
