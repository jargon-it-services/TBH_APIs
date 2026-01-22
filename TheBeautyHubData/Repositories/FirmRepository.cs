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
    /// <summary>
    /// Repository implementation for Firm entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class FirmRepository : IFirmRepository
    {
        private readonly BeautyHubDbContext _context;

        public FirmRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Firm> InsertFirmAsync(Firm firm)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountId", firm.AccountId),
                new SqlParameter("@FirmName", firm.FirmName),
                new SqlParameter("@FirmAddress", (object?)firm.FirmAddress ?? DBNull.Value),
                new SqlParameter("@FirmGstin", (object?)firm.FirmGstin ?? DBNull.Value),
                new SqlParameter("@FirmContact", (object?)firm.FirmContact ?? DBNull.Value),
                new SqlParameter("@FirmEmail", (object?)firm.FirmEmail ?? DBNull.Value),
                new SqlParameter("@FirmPhoto", (object?)firm.FirmPhoto ?? DBNull.Value),
                new SqlParameter("@FirmOwnerName", (object?)firm.FirmOwnerName ?? DBNull.Value),
                new SqlParameter("@FirmType", (object?)firm.FirmType ?? DBNull.Value),
                new SqlParameter("@FirmRegistration", (object?)firm.FirmRegistration ?? DBNull.Value),
                new SqlParameter("@FirmLogo", (object?)firm.FirmLogo ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)firm.CreatedBy ?? DBNull.Value)
            };

            var result = await _context.Firms
                .FromSqlRaw("EXEC usp_Insert_Firm @AccountId, @FirmName, @FirmAddress, @FirmGstin, @FirmContact, @FirmEmail, @FirmPhoto, @FirmOwnerName, @FirmType, @FirmRegistration, @FirmLogo, @CreatedBy",
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? firm;
        }

        public async Task<Firm> UpdateFirmAsync(Firm firm)
        {
            var parameters = new[]
            {
                new SqlParameter("@FirmId", firm.FirmId),
                new SqlParameter("@FirmName", firm.FirmName),
                new SqlParameter("@FirmAddress", (object?)firm.FirmAddress ?? DBNull.Value),
                new SqlParameter("@FirmGstin", (object?)firm.FirmGstin ?? DBNull.Value),
                new SqlParameter("@FirmContact", (object?)firm.FirmContact ?? DBNull.Value),
                new SqlParameter("@FirmEmail", (object?)firm.FirmEmail ?? DBNull.Value),
                new SqlParameter("@FirmPhoto", (object?)firm.FirmPhoto ?? DBNull.Value),
                new SqlParameter("@FirmOwnerName", (object?)firm.FirmOwnerName ?? DBNull.Value),
                new SqlParameter("@FirmType", (object?)firm.FirmType ?? DBNull.Value),
                new SqlParameter("@FirmRegistration", (object?)firm.FirmRegistration ?? DBNull.Value),
                new SqlParameter("@FirmLogo", (object?)firm.FirmLogo ?? DBNull.Value)
            };

            var result = await _context.Firms
                .FromSqlRaw("EXEC usp_Update_Firm @FirmId, @FirmName, @FirmAddress, @FirmGstin, @FirmContact, @FirmEmail, @FirmPhoto, @FirmOwnerName, @FirmType, @FirmRegistration, @FirmLogo",
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? firm;
        }

        public async Task<int> DeleteFirmAsync(Guid firmId)
        {
            var parameter = new SqlParameter("@FirmId", firmId);
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Delete_Firm @FirmId", parameter);
        }

        public async Task<Firm?> GetFirmByIdAsync(Guid firmId)
        {
            var parameter = new SqlParameter("@FirmId", firmId);
            var result = await _context.Firms
                .FromSqlRaw("EXEC usp_Get_FirmById @FirmId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Firm>> GetAllFirmsAsync()
        {
            return await _context.Firms
                .FromSqlRaw("EXEC usp_Get_AllFirms")
                .ToListAsync();
        }

        public async Task<IEnumerable<Firm>> GetFirmsByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Firms
                .FromSqlRaw("EXEC usp_Get_FirmsByAccountId @AccountId", parameter)
                .ToListAsync();
        }
    }
}
