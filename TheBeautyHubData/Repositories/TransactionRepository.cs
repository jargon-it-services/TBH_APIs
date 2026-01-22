using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BeautyHubDbContext _context;

        public TransactionRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> InsertAsync(Transaction transaction)
        {
            var parameters = new[]
            {
                new SqlParameter("@Status", transaction.Status ?? "Draft"),
                new SqlParameter("@TotalAmount", transaction.TotalAmount),
                new SqlParameter("@AccountId", transaction.AccountId),
                new SqlParameter("@FirmId", (object?)transaction.FirmId ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)transaction.CreatedBy ?? DBNull.Value),
                new SqlParameter("@PostedDate", (object?)transaction.PostedDate ?? DBNull.Value),
                new SqlParameter("@TransactionDate", (object?)transaction.TransactionDate ?? DBNull.Value),
                new SqlParameter("@CheckInTime", (object?)transaction.CheckInTime ?? DBNull.Value),
                new SqlParameter("@CheckOutTime", (object?)transaction.CheckOutTime ?? DBNull.Value)
            };

            var result = await _context.Transactions
                .FromSqlRaw("EXEC usp_Insert_Transaction @Status, @TotalAmount, @AccountId, @FirmId, @CreatedBy, @PostedDate, @TransactionDate, @CheckInTime, @CheckOutTime", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? transaction;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            var parameters = new[]
            {
                new SqlParameter("@TransactionId", transaction.TransactionId),
                new SqlParameter("@Status", transaction.Status ?? "Draft"),
                new SqlParameter("@TotalAmount", transaction.TotalAmount),
                new SqlParameter("@PostedDate", (object?)transaction.PostedDate ?? DBNull.Value),
                new SqlParameter("@TransactionDate", (object?)transaction.TransactionDate ?? DBNull.Value),
                new SqlParameter("@CheckInTime", (object?)transaction.CheckInTime ?? DBNull.Value),
                new SqlParameter("@CheckOutTime", (object?)transaction.CheckOutTime ?? DBNull.Value)
            };

            var result = await _context.Transactions
                .FromSqlRaw("EXEC usp_Update_Transaction @TransactionId, @Status, @TotalAmount, @PostedDate, @TransactionDate, @CheckInTime, @CheckOutTime", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? transaction;
        }

        public async Task<int> DeleteAsync(Guid transactionId)
        {
            var parameter = new SqlParameter("@TransactionId", transactionId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_Transaction @TransactionId", parameter);
        }

        public async Task<Transaction?> GetByIdAsync(Guid transactionId)
        {
            var parameter = new SqlParameter("@TransactionId", transactionId);
            var result = await _context.Transactions
                .FromSqlRaw("EXEC usp_Get_TransactionById @TransactionId", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .FromSqlRaw("EXEC usp_Get_AllTransactions")
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Transactions
                .FromSqlRaw("EXEC usp_Get_TransactionsByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByFirmIdAsync(Guid firmId)
        {
            var parameter = new SqlParameter("@FirmId", firmId);
            return await _context.Transactions
                .FromSqlRaw("EXEC usp_Get_TransactionsByFirmId @FirmId", parameter)
                .ToListAsync();
        }
    }
}
