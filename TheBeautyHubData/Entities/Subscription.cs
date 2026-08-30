using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBeautyHubData.Enums;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a subscription to a plan by an account.
    /// Tracks subscription status, amounts, discounts, and expiration.
    /// </summary>
    [Table("Subscription")]
    public class Subscription
    {
        /// <summary>
        /// Unique identifier for the subscription
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Foreign key to the Account subscribing
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Foreign key to the Plan being subscribed to
        /// </summary>
        [Required]
        public Guid PlanId { get; set; }

        /// <summary>
        /// Current status of the subscription
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = SubscriptionStatus.Pending.ToApiValue();

        /// <summary>
        /// Date and time when subscription expires
        /// </summary>
        public DateTime? ExpiryOn { get; set; }

        /// <summary>
        /// User ID who created this subscription
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when subscription was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Total subscription amount before discount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubscriptionAmount { get; set; }

        /// <summary>
        /// Discount amount applied to the subscription
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountedAmount { get; set; } = 0;

        /// <summary>
        /// Final amount after applying discount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubscriptionAmountAfterDiscount { get; set; }

        /// <summary>
        /// Type of discount applied (Wallet or Coupon)
        /// </summary>
        [StringLength(20)]
        public string? DiscountType { get; set; }

        // Navigation properties
        /// <summary>
        /// The account this subscription belongs to
        /// </summary>

        /// <summary>
        /// The plan this subscription is for
        /// </summary>
        [ForeignKey("PlanId")]
        public virtual Plans Plan { get; set; } = null!;
    }
}
