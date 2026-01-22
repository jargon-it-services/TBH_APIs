using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Service interface for TransactionRules operations.
    /// Defines business logic layer contract for transaction rules management.
    /// </summary>
    public interface ITransactionRulesService
    {
        Task<TransactionRulesDto> CreateTransactionRulesAsync(CreateTransactionRulesDto createTransactionRulesDto);
        Task<TransactionRulesDto> UpdateTransactionRulesAsync(UpdateTransactionRulesDto updateTransactionRulesDto);
        Task<bool> DeleteTransactionRulesAsync(Guid transactionRuleId);
        Task<TransactionRulesDto?> GetTransactionRulesByIdAsync(Guid transactionRuleId);
        Task<IEnumerable<TransactionRulesDto>> GetTransactionRulesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<TransactionRulesDto>> GetAllTransactionRulesAsync();
    }
}
