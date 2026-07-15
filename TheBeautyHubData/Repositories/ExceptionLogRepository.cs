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
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        private readonly BeautyHubDbContext _context;

        public ExceptionLogRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<ExceptionLog> InsertAsync(ExceptionLog exceptionLog)
        {
            _context.ExceptionLogs.Add(exceptionLog);
            await _context.SaveChangesAsync();
            return exceptionLog;
        }

        public async Task<int> DeleteAsync(long exceptionLogId)
        {
            var exceptionLog = await _context.ExceptionLogs.FindAsync(exceptionLogId);
            if (exceptionLog == null) return 0;
            
            _context.ExceptionLogs.Remove(exceptionLog);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<ExceptionLog?> GetByIdAsync(long exceptionLogId)
        {
            return await _context.ExceptionLogs.FindAsync(exceptionLogId);
        }

        public async Task<IEnumerable<ExceptionLog>> GetAllAsync(int pageNumber = 1, int pageSize = 100)
        {
            return await _context.ExceptionLogs
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExceptionLog>> GetByUserIdAsync(Guid userId, int pageNumber = 1, int pageSize = 100)
        {
            return await _context.ExceptionLogs
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExceptionLog>> GetByTypeAsync(string exceptionType, int pageNumber = 1, int pageSize = 100)
        {
            return await _context.ExceptionLogs
                .Where(e => e.ExceptionType == exceptionType)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> DeleteOldLogsAsync(int daysOld)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            var oldLogs = await _context.ExceptionLogs
                .Where(e => e.CreatedAt < cutoffDate)
                .ToListAsync();
            
            _context.ExceptionLogs.RemoveRange(oldLogs);
            await _context.SaveChangesAsync();
            return oldLogs.Count;
        }
    }
}
