using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly BeautyHubDbContext _context;

        public ReportRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Report> InsertAsync(Report report)
        {
            var parameters = new[]
            {
                new SqlParameter("@ReportName", report.ReportName),
                new SqlParameter("@IsActive", report.IsActive)
            };

            var result = await _context.Reports
                .FromSqlRaw("EXEC usp_Insert_Report @ReportName, @IsActive", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? report;
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            var parameters = new[]
            {
                new SqlParameter("@ReportId", report.ReportId),
                new SqlParameter("@ReportName", report.ReportName),
                new SqlParameter("@IsActive", report.IsActive)
            };

            var result = await _context.Reports
                .FromSqlRaw("EXEC usp_Update_Report @ReportId, @ReportName, @IsActive", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? report;
        }

        public async Task<int> DeleteAsync(Guid reportId)
        {
            var parameter = new SqlParameter("@ReportId", reportId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_Report @ReportId", parameter);
        }

        public async Task<Report?> GetByIdAsync(Guid reportId)
        {
            var parameter = new SqlParameter("@ReportId", reportId);
            var result = await _context.Reports
                .FromSqlRaw("EXEC usp_Get_ReportById @ReportId", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports
                .FromSqlRaw("EXEC usp_Get_AllReports")
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetActiveReportsAsync()
        {
            return await _context.Reports
                .FromSqlRaw("EXEC usp_Get_ActiveReports")
                .ToListAsync();
        }
    }
}
