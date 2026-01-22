using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IReportForAccountService
    {
        Task<ReportForAccountDto> CreateAsync(CreateReportForAccountDto dto);
        Task<ReportForAccountDto> UpdateAsync(Guid id, UpdateReportForAccountDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ReportForAccountDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReportForAccountDto>> GetByAccountIdAsync(Guid accountId);
    }
}
