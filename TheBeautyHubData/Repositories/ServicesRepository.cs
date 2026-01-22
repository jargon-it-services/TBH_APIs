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
    /// Repository implementation for Services entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class ServicesRepository : IServicesRepository
    {
        private readonly BeautyHubDbContext _context;

        public ServicesRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Services> InsertServicesAsync(Services services)
        {
            var parameters = new[]
            {
                new SqlParameter("@ServiceName", services.ServiceName),
                new SqlParameter("@ServiceDescription", (object?)services.ServiceDescription ?? DBNull.Value),
                new SqlParameter("@ServicePrice", services.ServicePrice),
                new SqlParameter("@ServiceTypeId", (object?)services.ServiceTypeId ?? DBNull.Value),
                new SqlParameter("@AccountId", services.AccountId),
                new SqlParameter("@FirmId", (object?)services.FirmId ?? DBNull.Value),
                new SqlParameter("@IsIncentiveApplicable", services.IsIncentiveApplicable),
                new SqlParameter("@IncentiveAmount", (object?)services.IncentiveAmount ?? DBNull.Value),
                new SqlParameter("@IncentivePercentage", (object?)services.IncentivePercentage ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)services.CreatedBy ?? DBNull.Value)
            };

            var result = await _context.Services
                .FromSqlRaw("EXEC usp_Insert_Services @ServiceName, @ServiceDescription, @ServicePrice, @ServiceTypeId, @AccountId, @FirmId, @IsIncentiveApplicable, @IncentiveAmount, @IncentivePercentage, @CreatedBy", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? services;
        }

        public async Task<Services> UpdateServicesAsync(Services services)
        {
            var parameters = new[]
            {
                new SqlParameter("@ServiceId", services.ServiceId),
                new SqlParameter("@ServiceName", services.ServiceName),
                new SqlParameter("@ServiceDescription", (object?)services.ServiceDescription ?? DBNull.Value),
                new SqlParameter("@ServicePrice", services.ServicePrice),
                new SqlParameter("@ServiceTypeId", (object?)services.ServiceTypeId ?? DBNull.Value),
                new SqlParameter("@FirmId", (object?)services.FirmId ?? DBNull.Value),
                new SqlParameter("@IsIncentiveApplicable", services.IsIncentiveApplicable),
                new SqlParameter("@IncentiveAmount", (object?)services.IncentiveAmount ?? DBNull.Value),
                new SqlParameter("@IncentivePercentage", (object?)services.IncentivePercentage ?? DBNull.Value)
            };

            var result = await _context.Services
                .FromSqlRaw("EXEC usp_Update_Services @ServiceId, @ServiceName, @ServiceDescription, @ServicePrice, @ServiceTypeId, @FirmId, @IsIncentiveApplicable, @IncentiveAmount, @IncentivePercentage", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? services;
        }

        public async Task<int> DeleteServicesAsync(Guid serviceId)
        {
            var parameter = new SqlParameter("@ServiceId", serviceId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_Services @ServiceId", parameter);
        }

        public async Task<Services?> GetServicesByIdAsync(Guid serviceId)
        {
            var parameter = new SqlParameter("@ServiceId", serviceId);
            var result = await _context.Services
                .FromSqlRaw("EXEC usp_Get_ServicesById @ServiceId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Services>> GetServicesByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Services
                .FromSqlRaw("EXEC usp_Get_ServicesByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Services>> GetAllServicesAsync()
        {
            return await _context.Services
                .FromSqlRaw("EXEC usp_Get_AllServices")
                .ToListAsync();
        }
    }
}
