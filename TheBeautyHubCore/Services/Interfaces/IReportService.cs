using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> CreateAsync(CreateReportDto dto);
        Task<ReportDto> UpdateAsync(Guid reportId, UpdateReportDto dto);
        Task<bool> DeleteAsync(Guid reportId);
        Task<ReportDto?> GetByIdAsync(Guid reportId);
        Task<IEnumerable<ReportDto>> GetAllAsync();
        Task<IEnumerable<ReportDto>> GetActiveReportsAsync();
    }
}
