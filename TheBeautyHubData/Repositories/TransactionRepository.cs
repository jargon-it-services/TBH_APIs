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

        public async Task<Transaction> InsertAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            transaction.LastUpdated = DateTime.UtcNow;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<int> DeleteAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return 0;
            
            transaction.IsDeleted = true;
            transaction.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<Transaction?> GetByIdAsync(Guid transactionId)
        {
            return await _context.Transactions
                .Where(t => t.TransactionId == transactionId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .Where(t => !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByFirmIdAsync(Guid firmId)
        {
            return await _context.Transactions
                .Where(t => t.FirmId == firmId && !t.IsDeleted)
                .ToListAsync();
        }
    }
}
