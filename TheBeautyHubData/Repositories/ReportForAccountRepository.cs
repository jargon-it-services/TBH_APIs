using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class ReportForAccountRepository : IReportForAccountRepository
    {
        private readonly BeautyHubDbContext _context;

        public ReportForAccountRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<ReportForAccount> InsertAsync(ReportForAccount reportForAccount)
        {
            var parameters = new[]
            {
                new SqlParameter("@ReportId", reportForAccount.ReportId),
                new SqlParameter("@AccountId", reportForAccount.AccountId),
                new SqlParameter("@IsActive", reportForAccount.IsActive),
                new SqlParameter("@CreatedBy", (object?)reportForAccount.CreatedBy ?? DBNull.Value)
            };

            var result = await _context.ReportsForAccount
                .FromSqlRaw("EXEC usp_Insert_ReportForAccount @ReportId, @AccountId, @IsActive, @CreatedBy", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? reportForAccount;
        }

        public async Task<ReportForAccount> UpdateAsync(ReportForAccount reportForAccount)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", reportForAccount.Id),
                new SqlParameter("@IsActive", reportForAccount.IsActive)
            };

            var result = await _context.ReportsForAccount
                .FromSqlRaw("EXEC usp_Update_ReportForAccount @Id, @IsActive", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? reportForAccount;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var parameter = new SqlParameter("@Id", id);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_ReportForAccount @Id", parameter);
        }

        public async Task<ReportForAccount?> GetByIdAsync(Guid id)
        {
            var parameter = new SqlParameter("@Id", id);
            var result = await _context.ReportsForAccount
                .FromSqlRaw("EXEC usp_Get_ReportForAccountById @Id", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<ReportForAccount>> GetByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.ReportsForAccount
                .FromSqlRaw("EXEC usp_Get_ReportsByAccountId @AccountId", parameter)
                .ToListAsync();
        }
    }
}
