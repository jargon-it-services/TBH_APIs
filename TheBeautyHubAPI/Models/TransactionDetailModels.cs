using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateTransactionDetailRequest
    {
        [Required]
        public Guid TransactionId { get; set; }
        
        [Required]
        public Guid TransactionTypeId { get; set; }
        
        public Guid? ExpensesTypeId { get; set; }
        public Guid? ServiceId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        public decimal? IncentiveAmount { get; set; }
        public Guid? TransactionRuleId { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateTransactionDetailRequest
    {
        [Required]
        public Guid TransactionTypeId { get; set; }
        
        public Guid? ExpensesTypeId { get; set; }
        public Guid? ServiceId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        public decimal? IncentiveAmount { get; set; }
        public Guid? TransactionRuleId { get; set; }
    }

    public class TransactionDetailResponse
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
}
