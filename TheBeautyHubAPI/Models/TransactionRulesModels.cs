using System;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new transaction rule.
    /// </summary>
    public class CreateTransactionRulesRequest
    {
        public string RuleName { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Request model for updating an existing transaction rule.
    /// </summary>
    public class UpdateTransactionRulesRequest
    {
        public string RuleName { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Response model for transaction rule information.
    /// </summary>
    public class TransactionRulesResponse
    {
        public Guid TransactionRuleId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsActive { get; set; }
    }
}
