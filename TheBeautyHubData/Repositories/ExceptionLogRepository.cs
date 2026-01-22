using Microsoft.Data.SqlClient;
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

        public async Task<long> InsertAsync(ExceptionLog log)
        {
            var parameters = new[]
            {
                new SqlParameter("@Type", log.Type),
                new SqlParameter("@ErrorMessage", log.ErrorMessage),
                new SqlParameter("@DeviceName", (object?)log.DeviceName ?? DBNull.Value),
                new SqlParameter("@UserId", (object?)log.UserId ?? DBNull.Value)
            };

            var result = await _context.Database
                .SqlQueryRaw<long>("EXEC usp_Insert_ExceptionLog @Type, @ErrorMessage, @DeviceName, @UserId", parameters)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<int> DeleteAsync(long id)
        {
            var parameter = new SqlParameter("@Id", id);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_ExceptionLog @Id", parameter);
        }

        public async Task<ExceptionLog?> GetByIdAsync(long id)
        {
            var parameter = new SqlParameter("@Id", id);
            var result = await _context.ExceptionLogs
                .FromSqlRaw("EXEC usp_Get_ExceptionLogById @Id", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<ExceptionLog>> GetAllAsync(int pageSize = 100, int pageNumber = 1)
        {
            var parameters = new[]
            {
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageNumber", pageNumber)
            };

            return await _context.ExceptionLogs
                .FromSqlRaw("EXEC usp_Get_AllExceptionLogs @PageSize, @PageNumber", parameters)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExceptionLog>> GetByUserIdAsync(Guid userId, int pageSize = 100, int pageNumber = 1)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageNumber", pageNumber)
            };

            return await _context.ExceptionLogs
                .FromSqlRaw("EXEC usp_Get_ExceptionLogsByUserId @UserId, @PageSize, @PageNumber", parameters)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExceptionLog>> GetByTypeAsync(string type, int pageSize = 100, int pageNumber = 1)
        {
            var parameters = new[]
            {
                new SqlParameter("@Type", type),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageNumber", pageNumber)
            };

            return await _context.ExceptionLogs
                .FromSqlRaw("EXEC usp_Get_ExceptionLogsByType @Type, @PageSize, @PageNumber", parameters)
                .ToListAsync();
        }

        public async Task<int> DeleteOldLogsAsync(int daysToKeep = 90)
        {
            var parameter = new SqlParameter("@DaysToKeep", daysToKeep);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_OldExceptionLogs @DaysToKeep", parameter);
        }
    }
}
