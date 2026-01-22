using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for TransactionRules repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface ITransactionRulesRepository
    {
        Task<TransactionRules> InsertTransactionRulesAsync(TransactionRules transactionRules);
        Task<TransactionRules> UpdateTransactionRulesAsync(TransactionRules transactionRules);
        Task<int> DeleteTransactionRulesAsync(Guid transactionRuleId);
        Task<TransactionRules?> GetTransactionRulesByIdAsync(Guid transactionRuleId);
        Task<IEnumerable<TransactionRules>> GetTransactionRulesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<TransactionRules>> GetAllTransactionRulesAsync();
    }
}
