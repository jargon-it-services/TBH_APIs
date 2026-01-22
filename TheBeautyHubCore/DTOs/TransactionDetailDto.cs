namespace TheBeautyHubCore.DTOs
{
    public class TransactionDetailDto
    {
        public Guid TransactionDetailsId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid TransactionTypeId { get; set; }
        public Guid? ExpensesTypeId { get; set; }
        public Guid? ServiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public Guid? TransactionRuleId { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CreateTransactionDetailDto
    {
        public Guid TransactionId { get; set; }
        public Guid TransactionTypeId { get; set; }
        public Guid? ExpensesTypeId { get; set; }
        public Guid? ServiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public Guid? TransactionRuleId { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateTransactionDetailDto
    {
        public Guid TransactionTypeId { get; set; }
        public Guid? ExpensesTypeId { get; set; }
        public Guid? ServiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public Guid? TransactionRuleId { get; set; }
    }
}
