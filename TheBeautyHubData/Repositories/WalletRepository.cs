using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    /// <summary>
    /// Repository implementation for Wallet entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class WalletRepository : IWalletRepository
    {
        private readonly BeautyHubDbContext _context;

        public WalletRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> InsertWalletAsync(Wallet wallet)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountId", wallet.AccountId),
                new SqlParameter("@Amount", wallet.Amount),
                new SqlParameter("@WalletType", wallet.WalletType),
                new SqlParameter("@IsUsed", wallet.IsUsed)
            };

            var result = await _context.Wallets
                .FromSqlRaw("EXEC usp_Insert_Wallet @AccountId, @Amount, @WalletType, @IsUsed", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? wallet;
        }

        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            var parameters = new[]
            {
                new SqlParameter("@WalletId", wallet.WalletId),
                new SqlParameter("@Amount", wallet.Amount),
                new SqlParameter("@WalletType", wallet.WalletType),
                new SqlParameter("@IsUsed", wallet.IsUsed)
            };

            var result = await _context.Wallets
                .FromSqlRaw("EXEC usp_Update_Wallet @WalletId, @Amount, @WalletType, @IsUsed", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? wallet;
        }

        public async Task<int> DeleteWalletAsync(Guid walletId)
        {
            var parameter = new SqlParameter("@WalletId", walletId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_Wallet @WalletId", parameter);
        }

        public async Task<Wallet?> GetWalletByIdAsync(Guid walletId)
        {
            var parameter = new SqlParameter("@WalletId", walletId);
            var result = await _context.Wallets
                .FromSqlRaw("EXEC usp_Get_WalletById @WalletId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Wallet>> GetWalletsByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Wallets
                .FromSqlRaw("EXEC usp_Get_WalletsByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Wallet>> GetAllWalletsAsync()
        {
            return await _context.Wallets
                .FromSqlRaw("EXEC usp_Get_AllWallets")
                .ToListAsync();
        }
    }
}
