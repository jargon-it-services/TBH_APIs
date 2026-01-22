using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
        Task<TransactionDto> UpdateAsync(Guid transactionId, UpdateTransactionDto dto);
        Task<bool> DeleteAsync(Guid transactionId);
        Task<TransactionDto?> GetByIdAsync(Guid transactionId);
        Task<IEnumerable<TransactionDto>> GetAllAsync();
        Task<IEnumerable<TransactionDto>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<TransactionDto>> GetByFirmIdAsync(Guid firmId);
    }
}
