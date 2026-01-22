using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Wallet repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IWalletRepository
    {
        Task<Wallet> InsertWalletAsync(Wallet wallet);
        Task<Wallet> UpdateWalletAsync(Wallet wallet);
        Task<int> DeleteWalletAsync(Guid walletId);
        Task<Wallet?> GetWalletByIdAsync(Guid walletId);
        Task<IEnumerable<Wallet>> GetWalletsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Wallet>> GetAllWalletsAsync();
    }
}
