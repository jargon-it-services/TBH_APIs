using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface ITransactionDetailRepository
    {
        Task<TransactionDetail> InsertAsync(TransactionDetail detail);
        Task<TransactionDetail> UpdateAsync(TransactionDetail detail);
        Task<int> DeleteAsync(Guid transactionDetailsId);
        Task<TransactionDetail?> GetByIdAsync(Guid transactionDetailsId);
        Task<IEnumerable<TransactionDetail>> GetByTransactionIdAsync(Guid transactionId);
    }
}
