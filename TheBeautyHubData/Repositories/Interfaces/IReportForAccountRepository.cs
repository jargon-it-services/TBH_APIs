using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IReportForAccountRepository
    {
        Task<ReportForAccount> InsertAsync(ReportForAccount reportForAccount);
        Task<ReportForAccount> UpdateAsync(ReportForAccount reportForAccount);
        Task<int> DeleteAsync(Guid id);
        Task<ReportForAccount?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReportForAccount>> GetByAccountIdAsync(Guid accountId);
    }
}
