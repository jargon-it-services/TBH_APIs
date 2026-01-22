using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IExceptionLogRepository
    {
        Task<long> InsertAsync(ExceptionLog log);
        Task<int> DeleteAsync(long id);
        Task<ExceptionLog?> GetByIdAsync(long id);
        Task<IEnumerable<ExceptionLog>> GetAllAsync(int pageSize = 100, int pageNumber = 1);
        Task<IEnumerable<ExceptionLog>> GetByUserIdAsync(Guid userId, int pageSize = 100, int pageNumber = 1);
        Task<IEnumerable<ExceptionLog>> GetByTypeAsync(string type, int pageSize = 100, int pageNumber = 1);
        Task<int> DeleteOldLogsAsync(int daysToKeep = 90);
    }
}
