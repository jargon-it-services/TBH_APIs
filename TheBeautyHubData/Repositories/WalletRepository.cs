using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly BeautyHubDbContext _context;

        public WalletRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> InsertWalletAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<int> DeleteWalletAsync(Guid walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet == null) return 0;
            
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<Wallet?> GetWalletByIdAsync(Guid walletId)
        {
            return await _context.Wallets.FindAsync(walletId);
        }

        public async Task<IEnumerable<Wallet>> GetAllWalletsAsync()
        {
            return await _context.Wallets.ToListAsync();
        }

        public async Task<IEnumerable<Wallet>> GetWalletsByAccountIdAsync(Guid accountId)
        {
            return await _context.Wallets
                .Where(w => w.AccountId == accountId)
                .ToListAsync();
        }
    }
}
