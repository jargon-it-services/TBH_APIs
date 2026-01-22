using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Service interface for TransactionType operations.
    /// Defines business logic layer contract for transaction type management.
    /// </summary>
    public interface ITransactionTypeService
    {
        Task<TransactionTypeDto> CreateTransactionTypeAsync(CreateTransactionTypeDto createTransactionTypeDto);
        Task<TransactionTypeDto> UpdateTransactionTypeAsync(UpdateTransactionTypeDto updateTransactionTypeDto);
        Task<bool> DeleteTransactionTypeAsync(Guid transactionTypeId);
        Task<TransactionTypeDto?> GetTransactionTypeByIdAsync(Guid transactionTypeId);
        Task<IEnumerable<TransactionTypeDto>> GetAllTransactionTypesAsync();
        Task<IEnumerable<TransactionTypeDto>> GetActiveTransactionTypesAsync();
    }
}
