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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BeautyHubDbContext _context;

        public TransactionRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> InsertTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            transaction.LastUpdated = DateTime.UtcNow;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<int> DeleteTransactionAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return 0;
            
            transaction.IsDeleted = true;
            transaction.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId)
        {
            return await _context.Transactions
                .Where(t => t.TransactionId == transactionId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Where(t => !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId && !t.IsDeleted)
                .ToListAsync();
        }
    }
}
