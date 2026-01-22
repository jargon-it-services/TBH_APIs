using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class FirmDetailsRepository : IFirmDetailsRepository
    {
        private readonly BeautyHubDbContext _context;

        public FirmDetailsRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<FirmDetails> InsertFirmDetailsAsync(FirmDetails firmDetails)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", firmDetails.UserId),
                new SqlParameter("@AccountId", firmDetails.AccountId),
                new SqlParameter("@FirmId", firmDetails.FirmId)
            };

            var result = await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Insert_FirmDetails @UserId, @AccountId, @FirmId", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? firmDetails;
        }

        public async Task<FirmDetails> UpdateFirmDetailsAsync(FirmDetails firmDetails)
        {
            var parameters = new[]
            {
                new SqlParameter("@FirmDetailsId", firmDetails.FirmDetailsId),
                new SqlParameter("@UserId", firmDetails.UserId),
                new SqlParameter("@AccountId", firmDetails.AccountId),
                new SqlParameter("@FirmId", firmDetails.FirmId)
            };

            var result = await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Update_FirmDetails @FirmDetailsId, @UserId, @AccountId, @FirmId", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? firmDetails;
        }

        public async Task<int> DeleteFirmDetailsAsync(Guid firmDetailsId)
        {
            var parameter = new SqlParameter("@FirmDetailsId", firmDetailsId);
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Delete_FirmDetails @FirmDetailsId", parameter);
        }

        public async Task<FirmDetails?> GetFirmDetailsByIdAsync(Guid firmDetailsId)
        {
            var parameter = new SqlParameter("@FirmDetailsId", firmDetailsId);
            var result = await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Get_FirmDetailsById @FirmDetailsId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<FirmDetails>> GetAllFirmDetailsAsync()
        {
            return await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Get_AllFirmDetails")
                .ToListAsync();
        }

        public async Task<IEnumerable<FirmDetails>> GetFirmDetailsByFirmIdAsync(Guid firmId)
        {
            var parameter = new SqlParameter("@FirmId", firmId);
            return await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Get_FirmDetailsByFirmId @FirmId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<FirmDetails>> GetFirmDetailsByUserIdAsync(Guid userId)
        {
            var parameter = new SqlParameter("@UserId", userId);
            return await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Get_FirmDetailsByUserId @UserId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<FirmDetails>> GetFirmDetailsByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.FirmDetails
                .FromSqlRaw("EXEC usp_Get_FirmDetailsByAccountId @AccountId", parameter)
                .ToListAsync();
        }
    }
}
