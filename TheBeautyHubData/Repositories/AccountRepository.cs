using System;
using System.Collections.Generic;
using System.Data;
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
    /// Repository implementation for Account entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly BeautyHubDbContext _context;

        public AccountRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inserts a new account using the usp_Insert_Account stored procedure
        /// </summary>
        public async Task<Account> InsertAccountAsync(Account account)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountCode", account.AccountCode),
                new SqlParameter("@AccountName", account.AccountName),
                new SqlParameter("@AccountType", account.AccountType),
                new SqlParameter("@Mode", account.Mode),
                new SqlParameter("@IsUnderTrial", account.IsUnderTrial),
                new SqlParameter("@TrialStartedOn", (object?)account.TrialStartedOn ?? DBNull.Value),
                new SqlParameter("@TrialDuration", (object?)account.TrialDuration ?? DBNull.Value),
                new SqlParameter("@TrialExpiredOn", (object?)account.TrialExpiredOn ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)account.CreatedBy ?? DBNull.Value)
            };

            var result = await _context.Accounts
                .FromSqlRaw("EXEC usp_Insert_Account @AccountCode, @AccountName, @AccountType, @Mode, @IsUnderTrial, @TrialStartedOn, @TrialDuration, @TrialExpiredOn, @CreatedBy", 
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? account;
        }

        /// <summary>
        /// Updates an existing account using the usp_Update_Account stored procedure
        /// </summary>
        public async Task<Account> UpdateAccountAsync(Account account)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountId", account.AccountId),
                new SqlParameter("@AccountCode", account.AccountCode),
                new SqlParameter("@AccountName", account.AccountName),
                new SqlParameter("@AccountType", account.AccountType),
                new SqlParameter("@Mode", account.Mode),
                new SqlParameter("@IsUnderTrial", account.IsUnderTrial),
                new SqlParameter("@TrialStartedOn", (object?)account.TrialStartedOn ?? DBNull.Value),
                new SqlParameter("@TrialDuration", (object?)account.TrialDuration ?? DBNull.Value),
                new SqlParameter("@TrialExpiredOn", (object?)account.TrialExpiredOn ?? DBNull.Value)
            };

            var result = await _context.Accounts
                .FromSqlRaw("EXEC usp_Update_Account @AccountId, @AccountCode, @AccountName, @AccountType, @Mode, @IsUnderTrial, @TrialStartedOn, @TrialDuration, @TrialExpiredOn", 
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? account;
        }

        /// <summary>
        /// Soft deletes an account using the usp_Delete_Account stored procedure
        /// </summary>
        public async Task<int> DeleteAccountAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Delete_Account @AccountId", parameter);
        }

        /// <summary>
        /// Retrieves an account by ID using the usp_Get_AccountById stored procedure
        /// </summary>
        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            
            var result = await _context.Accounts
                .FromSqlRaw("EXEC usp_Get_AccountById @AccountId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all non-deleted accounts using the usp_Get_AllAccounts stored procedure
        /// </summary>
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .FromSqlRaw("EXEC usp_Get_AllAccounts")
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves an account by its unique code using the usp_Get_AccountByCode stored procedure
        /// </summary>
        public async Task<Account?> GetAccountByCodeAsync(string accountCode)
        {
            var parameter = new SqlParameter("@AccountCode", accountCode);
            
            var result = await _context.Accounts
                .FromSqlRaw("EXEC usp_Get_AccountByCode @AccountCode", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}
