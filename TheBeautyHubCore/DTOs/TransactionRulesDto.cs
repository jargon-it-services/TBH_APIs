using System;

namespace TheBeautyHubCore.DTOs
{
    /// <summary>
    /// DTO for TransactionRules entity.
    /// Used for retrieving transaction rules information.
    /// </summary>
    public class TransactionRulesDto
    {
        public Guid TransactionRuleId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for creating a new transaction rule.
    /// </summary>
    public class CreateTransactionRulesDto
    {
        public string RuleName { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing transaction rule.
    /// </summary>
    public class UpdateTransactionRulesDto
    {
        public Guid TransactionRuleId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsActive { get; set; }
    }
}
