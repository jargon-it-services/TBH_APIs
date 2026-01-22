using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> InsertAsync(Transaction transaction);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<int> DeleteAsync(Guid transactionId);
        Task<Transaction?> GetByIdAsync(Guid transactionId);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetByFirmIdAsync(Guid firmId);
    }
}
