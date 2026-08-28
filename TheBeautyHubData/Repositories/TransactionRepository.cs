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
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId && !t.IsDeleted);
        }

        public async Task<Transaction?> GetDetailsAsync(Guid transactionId, Guid accountId)
        {
            return await _context.Transactions
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Service)
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.ExpensesType)
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Staff)
                .Include(t => t.Branch)
                .Include(t => t.Staff)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.AccountId == accountId && !t.IsDeleted);
        }

        public async Task<Transaction?> GetByCodeAsync(string code, Guid accountId)
        {
            return await _context.Transactions
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Service)
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.ExpensesType)
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Staff)
                .Include(t => t.Branch)
                .Include(t => t.Staff)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.AccountId == accountId && t.Code == code && !t.IsDeleted);
        }

        public async Task<Transaction?> GetByIdempotencyKeyAsync(string idempotencyKey, Guid accountId)
        {
            return await _context.Transactions
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Service)
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.ExpensesType)
                .Include(t => t.Branch)
                .Include(t => t.Staff)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t =>
                    t.AccountId == accountId &&
                    t.IdempotencyKey == idempotencyKey &&
                    !t.IsDeleted);
        }

        public async Task<Transaction?> GetLatestByUserAsync(Guid accountId, Guid userId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.AccountId == accountId && t.CreatedBy == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountByAccountAsync(Guid accountId)
        {
            return await _context.Transactions.CountAsync(t => t.AccountId == accountId);
        }

        public async Task<IReadOnlyList<Transaction>> GetListByAccountAsync(Guid accountId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.Service)
                .Include(t => t.TransactionDetails.Where(d => !d.IsDeleted))
                    .ThenInclude(d => d.ExpensesType)
                .Include(t => t.Branch)
                .Include(t => t.Staff)
                .Where(t => t.AccountId == accountId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<IReadOnlyDictionary<Guid, int>> GetServiceUsageCountsAsync(Guid accountId)
        {
            return await _context.TransactionDetails
                .AsNoTracking()
                .Where(d => !d.IsDeleted && d.ServiceId != null && d.Transaction.AccountId == accountId && !d.Transaction.IsDeleted)
                .GroupBy(d => d.ServiceId!.Value)
                .Select(g => new { ServiceId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ServiceId, x => x.Count);
        }

        public async Task ReplaceDetailsAsync(Guid transactionId, IEnumerable<TransactionDetail> details)
        {
            var existing = await _context.TransactionDetails
                .Where(d => d.TransactionId == transactionId)
                .ToListAsync();

            _context.TransactionDetails.RemoveRange(existing);
            foreach (var detail in details)
            {
                detail.TransactionId = transactionId;
                _context.TransactionDetails.Add(detail);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.Where(t => !t.IsDeleted).ToListAsync();
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
