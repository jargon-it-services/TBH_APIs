using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> InsertAsync(Report report);
        Task<Report> UpdateAsync(Report report);
        Task<int> DeleteAsync(Guid reportId);
        Task<Report?> GetByIdAsync(Guid reportId);
        Task<IEnumerable<Report>> GetAllAsync();
        Task<IEnumerable<Report>> GetActiveReportsAsync();
    }
}
