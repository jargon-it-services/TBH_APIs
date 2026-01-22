using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly BeautyHubDbContext _context;

        public PartnerRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Partner> InsertAsync(Partner partner)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", partner.Name),
                new SqlParameter("@Type", (object?)partner.Type ?? DBNull.Value),
                new SqlParameter("@Address", (object?)partner.Address ?? DBNull.Value),
                new SqlParameter("@Mobile", (object?)partner.Mobile ?? DBNull.Value),
                new SqlParameter("@Email", (object?)partner.Email ?? DBNull.Value),
                new SqlParameter("@DateofBirth", (object?)partner.DateofBirth ?? DBNull.Value),
                new SqlParameter("@Gender", (object?)partner.Gender ?? DBNull.Value),
                new SqlParameter("@AccountId", (object?)partner.AccountId ?? DBNull.Value)
            };

            var result = await _context.Partners
                .FromSqlRaw("EXEC usp_Insert_Partner @Name, @Type, @Address, @Mobile, @Email, @DateofBirth, @Gender, @AccountId", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? partner;
        }

        public async Task<Partner> UpdateAsync(Partner partner)
        {
            var parameters = new[]
            {
                new SqlParameter("@PartnerId", partner.PartnerId),
                new SqlParameter("@Name", partner.Name),
                new SqlParameter("@Type", (object?)partner.Type ?? DBNull.Value),
                new SqlParameter("@Address", (object?)partner.Address ?? DBNull.Value),
                new SqlParameter("@Mobile", (object?)partner.Mobile ?? DBNull.Value),
                new SqlParameter("@Email", (object?)partner.Email ?? DBNull.Value),
                new SqlParameter("@DateofBirth", (object?)partner.DateofBirth ?? DBNull.Value),
                new SqlParameter("@Gender", (object?)partner.Gender ?? DBNull.Value)
            };

            var result = await _context.Partners
                .FromSqlRaw("EXEC usp_Update_Partner @PartnerId, @Name, @Type, @Address, @Mobile, @Email, @DateofBirth, @Gender", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? partner;
        }

        public async Task<int> DeleteAsync(Guid partnerId)
        {
            var parameter = new SqlParameter("@PartnerId", partnerId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_Partner @PartnerId", parameter);
        }

        public async Task<Partner?> GetByIdAsync(Guid partnerId)
        {
            var parameter = new SqlParameter("@PartnerId", partnerId);
            var result = await _context.Partners
                .FromSqlRaw("EXEC usp_Get_PartnerById @PartnerId", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _context.Partners
                .FromSqlRaw("EXEC usp_Get_AllPartners")
                .ToListAsync();
        }

        public async Task<IEnumerable<Partner>> GetByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Partners
                .FromSqlRaw("EXEC usp_Get_PartnersByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<Partner?> GetByEmailAsync(string email)
        {
            var parameter = new SqlParameter("@Email", email);
            var result = await _context.Partners
                .FromSqlRaw("EXEC usp_Get_PartnerByEmail @Email", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }
    }
}
