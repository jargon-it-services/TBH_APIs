using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface ITransactionDetailService
    {
        Task<TransactionDetailDto> CreateAsync(CreateTransactionDetailDto dto);
        Task<TransactionDetailDto> UpdateAsync(Guid transactionDetailsId, UpdateTransactionDetailDto dto);
        Task<bool> DeleteAsync(Guid transactionDetailsId);
        Task<TransactionDetailDto?> GetByIdAsync(Guid transactionDetailsId);
        Task<IEnumerable<TransactionDetailDto>> GetByTransactionIdAsync(Guid transactionId);
    }
}
