using System;

namespace TheBeautyHubCore.DTOs
{
    public class SubscriptionDto
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

    public class CreateSubscriptionDto
    {
        public Guid AccountId { get; set; }
        public Guid PlanId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpiryOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal SubscriptionAmountAfterDiscount { get; set; }
        public string? DiscountType { get; set; }
    }

    public class UpdateSubscriptionDto
    {
        public Guid SubscriptionId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpiryOn { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal SubscriptionAmountAfterDiscount { get; set; }
        public string? DiscountType { get; set; }
    }
}
