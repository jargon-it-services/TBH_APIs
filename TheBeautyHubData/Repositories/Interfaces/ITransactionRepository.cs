using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> InsertAsync(Transaction transaction);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<int> DeleteAsync(Guid transactionId);
        Task<Transaction?> GetByIdAsync(Guid transactionId);
        Task<Transaction?> GetDetailsAsync(Guid transactionId, Guid accountId);
        Task<Transaction?> GetByCodeAsync(string code, Guid accountId);
        Task<Transaction?> GetByIdempotencyKeyAsync(string idempotencyKey, Guid accountId);
        Task<Transaction?> GetLatestByUserAsync(Guid accountId, Guid userId);
        Task<int> CountByAccountAsync(Guid accountId);
        Task<IReadOnlyList<Transaction>> GetListByAccountAsync(Guid accountId);
        Task<IReadOnlyDictionary<Guid, int>> GetServiceUsageCountsAsync(Guid accountId);
        Task ReplaceDetailsAsync(Guid transactionId, IEnumerable<TransactionDetail> details);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetByFirmIdAsync(Guid firmId);
    }
}
