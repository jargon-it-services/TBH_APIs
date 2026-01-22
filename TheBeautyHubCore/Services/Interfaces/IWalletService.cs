using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Service interface for Wallet operations.
    /// Defines business logic layer contract for wallet management.
    /// </summary>
    public interface IWalletService
    {
        Task<WalletDto> CreateWalletAsync(CreateWalletDto createWalletDto);
        Task<WalletDto> UpdateWalletAsync(UpdateWalletDto updateWalletDto);
        Task<bool> DeleteWalletAsync(Guid walletId);
        Task<WalletDto?> GetWalletByIdAsync(Guid walletId);
        Task<IEnumerable<WalletDto>> GetWalletsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<WalletDto>> GetAllWalletsAsync();
    }
}
