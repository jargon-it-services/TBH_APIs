using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class TransactionDetailRepository : ITransactionDetailRepository
    {
        private readonly BeautyHubDbContext _context;

        public TransactionDetailRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionDetail> InsertAsync(TransactionDetail detail)
        {
            var parameters = new[]
            {
                new SqlParameter("@TransactionId", detail.TransactionId),
                new SqlParameter("@TransactionTypeId", detail.TransactionTypeId),
                new SqlParameter("@ExpensesTypeId", (object?)detail.ExpensesTypeId ?? DBNull.Value),
                new SqlParameter("@ServiceId", (object?)detail.ServiceId ?? DBNull.Value),
                new SqlParameter("@Amount", detail.Amount),
                new SqlParameter("@IncentiveAmount", (object?)detail.IncentiveAmount ?? DBNull.Value),
                new SqlParameter("@TransactionRuleId", (object?)detail.TransactionRuleId ?? DBNull.Value),
                new SqlParameter("@AccountId", (object?)detail.AccountId ?? DBNull.Value),
                new SqlParameter("@FirmId", (object?)detail.FirmId ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)detail.CreatedBy ?? DBNull.Value)
            };

            var result = await _context.TransactionDetails
                .FromSqlRaw("EXEC usp_Insert_TransactionDetail @TransactionId, @TransactionTypeId, @ExpensesTypeId, @ServiceId, @Amount, @IncentiveAmount, @TransactionRuleId, @AccountId, @FirmId, @CreatedBy", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? detail;
        }

        public async Task<TransactionDetail> UpdateAsync(TransactionDetail detail)
        {
            var parameters = new[]
            {
                new SqlParameter("@TransactionDetailsId", detail.TransactionDetailsId),
                new SqlParameter("@TransactionTypeId", detail.TransactionTypeId),
                new SqlParameter("@ExpensesTypeId", (object?)detail.ExpensesTypeId ?? DBNull.Value),
                new SqlParameter("@ServiceId", (object?)detail.ServiceId ?? DBNull.Value),
                new SqlParameter("@Amount", detail.Amount),
                new SqlParameter("@IncentiveAmount", (object?)detail.IncentiveAmount ?? DBNull.Value),
                new SqlParameter("@TransactionRuleId", (object?)detail.TransactionRuleId ?? DBNull.Value)
            };

            var result = await _context.TransactionDetails
                .FromSqlRaw("EXEC usp_Update_TransactionDetail @TransactionDetailsId, @TransactionTypeId, @ExpensesTypeId, @ServiceId, @Amount, @IncentiveAmount, @TransactionRuleId", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? detail;
        }

        public async Task<int> DeleteAsync(Guid transactionDetailsId)
        {
            var parameter = new SqlParameter("@TransactionDetailsId", transactionDetailsId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_TransactionDetail @TransactionDetailsId", parameter);
        }

        public async Task<TransactionDetail?> GetByIdAsync(Guid transactionDetailsId)
        {
            var parameter = new SqlParameter("@TransactionDetailsId", transactionDetailsId);
            var result = await _context.TransactionDetails
                .FromSqlRaw("EXEC usp_Get_TransactionDetailById @TransactionDetailsId", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TransactionDetail>> GetByTransactionIdAsync(Guid transactionId)
        {
            var parameter = new SqlParameter("@TransactionId", transactionId);
            return await _context.TransactionDetails
                .FromSqlRaw("EXEC usp_Get_TransactionDetailsByTransactionId @TransactionId", parameter)
                .ToListAsync();
        }
    }
}
