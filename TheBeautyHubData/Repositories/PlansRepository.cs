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
    public class PlansRepository : IPlansRepository
    {
        private readonly BeautyHubDbContext _context;

        public PlansRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Plans> InsertPlanAsync(Plans plan)
        {
            var parameters = new[]
            {
                new SqlParameter("@PlanName", plan.PlanName),
                new SqlParameter("@PlanDescription", (object?)plan.PlanDescription ?? DBNull.Value),
                new SqlParameter("@PlanCost", plan.PlanCost),
                new SqlParameter("@IsPlanActive", plan.IsPlanActive),
                new SqlParameter("@PlanAppliedTo", (object?)plan.PlanAppliedTo ?? DBNull.Value)
            };

            var result = await _context.Plans
                .FromSqlRaw("EXEC usp_Insert_Plan @PlanName, @PlanDescription, @PlanCost, @IsPlanActive, @PlanAppliedTo", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? plan;
        }

        public async Task<Plans> UpdatePlanAsync(Plans plan)
        {
            var parameters = new[]
            {
                new SqlParameter("@PlanId", plan.PlanId),
                new SqlParameter("@PlanName", plan.PlanName),
                new SqlParameter("@PlanDescription", (object?)plan.PlanDescription ?? DBNull.Value),
                new SqlParameter("@PlanCost", plan.PlanCost),
                new SqlParameter("@IsPlanActive", plan.IsPlanActive),
                new SqlParameter("@PlanAppliedTo", (object?)plan.PlanAppliedTo ?? DBNull.Value)
            };

            var result = await _context.Plans
                .FromSqlRaw("EXEC usp_Update_Plan @PlanId, @PlanName, @PlanDescription, @PlanCost, @IsPlanActive, @PlanAppliedTo", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? plan;
        }

        public async Task<int> DeletePlanAsync(Guid planId)
        {
            var parameter = new SqlParameter("@PlanId", planId);
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Delete_Plan @PlanId", parameter);
        }

        public async Task<Plans?> GetPlanByIdAsync(Guid planId)
        {
            var parameter = new SqlParameter("@PlanId", planId);
            var result = await _context.Plans
                .FromSqlRaw("EXEC usp_Get_PlanById @PlanId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Plans>> GetAllPlansAsync()
        {
            return await _context.Plans
                .FromSqlRaw("EXEC usp_Get_AllPlans")
                .ToListAsync();
        }

        public async Task<IEnumerable<Plans>> GetActivePlansAsync()
        {
            return await _context.Plans
                .FromSqlRaw("EXEC usp_Get_ActivePlans")
                .ToListAsync();
        }
    }
}
