using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionBootstrapDto> GetBootstrapAsync(Guid accountId, Guid userId, IReadOnlyList<string> roles);
        Task<TransactionSavedDto> CreateAsync(SaveTransactionDto dto);
        Task<TransactionSavedDto> UpdateAsync(string id, SaveTransactionDto dto);
        Task<TransactionSavedDto> MarkPaidAsync(string id, Guid accountId);
        Task<TransactionListDto> GetListAsync(Guid accountId);
        Task<TransactionRecordDto?> GetDetailsAsync(string id, Guid accountId);
    }
}
