using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for TransactionType repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface ITransactionTypeRepository
    {
        Task<TransactionType> InsertTransactionTypeAsync(TransactionType transactionType);
        Task<TransactionType> UpdateTransactionTypeAsync(TransactionType transactionType);
        Task<int> DeleteTransactionTypeAsync(Guid transactionTypeId);
        Task<TransactionType?> GetTransactionTypeByIdAsync(Guid transactionTypeId);
        Task<IEnumerable<TransactionType>> GetAllTransactionTypesAsync();
        Task<IEnumerable<TransactionType>> GetActiveTransactionTypesAsync();
    }
}
