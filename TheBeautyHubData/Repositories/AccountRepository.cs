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
    /// <summary>
    /// Repository implementation for Account entity.
    /// Uses EF Core for all CRUD operations.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly BeautyHubDbContext _context;

        public AccountRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inserts a new account
        /// </summary>
        public async Task<Account> InsertAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        /// <summary>
        /// Updates an existing account
        /// </summary>
        public async Task<Account> UpdateAccountAsync(Account account)
        {
            account.LastUpdated = DateTime.UtcNow;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        /// <summary>
        /// Soft deletes an account
        /// </summary>
        public async Task<int> DeleteAccountAsync(Guid accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return 0;

            account.IsDeleted = true;
            account.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return 1;
        }

        /// <summary>
        /// Retrieves an account by ID
        /// </summary>
        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            return await _context.Accounts
                .Where(a => a.AccountId == accountId && !a.IsDeleted)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all non-deleted accounts
        /// </summary>
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves an account by its unique code
        /// </summary>
        public async Task<Account?> GetAccountByCodeAsync(string accountCode)
        {
            return await _context.Accounts
                .Where(a => a.AccountCode == accountCode && !a.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
