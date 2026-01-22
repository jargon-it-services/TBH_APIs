using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Plans repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IPlansRepository
    {
        Task<Plans> InsertPlanAsync(Plans plan);
        Task<Plans> UpdatePlanAsync(Plans plan);
        Task<int> DeletePlanAsync(Guid planId);
        Task<Plans?> GetPlanByIdAsync(Guid planId);
        Task<IEnumerable<Plans>> GetAllPlansAsync();
        Task<IEnumerable<Plans>> GetActivePlansAsync();
    }
}
