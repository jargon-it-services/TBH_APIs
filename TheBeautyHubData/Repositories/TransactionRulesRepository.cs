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
    /// Repository implementation for TransactionRules entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class TransactionRulesRepository : ITransactionRulesRepository
    {
        private readonly BeautyHubDbContext _context;

        public TransactionRulesRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionRules> InsertTransactionRulesAsync(TransactionRules transactionRules)
        {
            var parameters = new[]
            {
                new SqlParameter("@RuleName", transactionRules.RuleName),
                new SqlParameter("@AccountId", (object?)transactionRules.AccountId ?? DBNull.Value),
                new SqlParameter("@FirmId", (object?)transactionRules.FirmId ?? DBNull.Value),
                new SqlParameter("@IsActive", transactionRules.IsActive)
            };

            var result = await _context.TransactionRules
                .FromSqlRaw("EXEC usp_Insert_TransactionRules @RuleName, @AccountId, @FirmId, @IsActive", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? transactionRules;
        }

        public async Task<TransactionRules> UpdateTransactionRulesAsync(TransactionRules transactionRules)
        {
            var parameters = new[]
            {
                new SqlParameter("@TransactionRuleId", transactionRules.TransactionRuleId),
                new SqlParameter("@RuleName", transactionRules.RuleName),
                new SqlParameter("@AccountId", (object?)transactionRules.AccountId ?? DBNull.Value),
                new SqlParameter("@FirmId", (object?)transactionRules.FirmId ?? DBNull.Value),
                new SqlParameter("@IsActive", transactionRules.IsActive)
            };

            var result = await _context.TransactionRules
                .FromSqlRaw("EXEC usp_Update_TransactionRules @TransactionRuleId, @RuleName, @AccountId, @FirmId, @IsActive", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? transactionRules;
        }

        public async Task<int> DeleteTransactionRulesAsync(Guid transactionRuleId)
        {
            var parameter = new SqlParameter("@TransactionRuleId", transactionRuleId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_TransactionRules @TransactionRuleId", parameter);
        }

        public async Task<TransactionRules?> GetTransactionRulesByIdAsync(Guid transactionRuleId)
        {
            var parameter = new SqlParameter("@TransactionRuleId", transactionRuleId);
            var result = await _context.TransactionRules
                .FromSqlRaw("EXEC usp_Get_TransactionRulesById @TransactionRuleId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TransactionRules>> GetTransactionRulesByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.TransactionRules
                .FromSqlRaw("EXEC usp_Get_TransactionRulesByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionRules>> GetAllTransactionRulesAsync()
        {
            return await _context.TransactionRules
                .FromSqlRaw("EXEC usp_Get_AllTransactionRules")
                .ToListAsync();
        }
    }
}
