using System;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IManagementService
    {
        Task<AccountSummaryDto> GetAccountSummaryAsync(Guid accountId);
        Task<FeatureLockDto> GetFeatureLockAsync(Guid accountId);
    }
}
