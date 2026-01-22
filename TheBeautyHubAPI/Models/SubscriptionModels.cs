using System;
using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateSubscriptionRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid PlanId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public DateTime? ExpiryOn { get; set; }

        public Guid? CreatedBy { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SubscriptionAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DiscountedAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SubscriptionAmountAfterDiscount { get; set; }

        [StringLength(20)]
        public string? DiscountType { get; set; }
    }

    public class UpdateSubscriptionRequest
    {
        [Required]
        public Guid SubscriptionId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public DateTime? ExpiryOn { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SubscriptionAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DiscountedAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SubscriptionAmountAfterDiscount { get; set; }

        [StringLength(20)]
        public string? DiscountType { get; set; }
    }

    public class SubscriptionResponse
    {
        public Guid SubscriptionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PlanId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpiryOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal SubscriptionAmountAfterDiscount { get; set; }
        public string? DiscountType { get; set; }
    }
}
