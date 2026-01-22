using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IExceptionLogService
    {
        /// <summary>
        /// Logs an exception to the database. Use this method from controllers to log exceptions.
        /// </summary>
        Task<long> LogExceptionAsync(Exception exception, Guid? userId = null, string? additionalInfo = null);
        
        Task<long> CreateAsync(CreateExceptionLogDto dto);
        Task<bool> DeleteAsync(long id);
        Task<ExceptionLogDto?> GetByIdAsync(long id);
        Task<IEnumerable<ExceptionLogDto>> GetAllAsync(int pageSize = 100, int pageNumber = 1);
        Task<IEnumerable<ExceptionLogDto>> GetByUserIdAsync(Guid userId, int pageSize = 100, int pageNumber = 1);
        Task<IEnumerable<ExceptionLogDto>> GetByTypeAsync(string type, int pageSize = 100, int pageNumber = 1);
        Task<int> DeleteOldLogsAsync(int daysToKeep = 90);
    }
}
