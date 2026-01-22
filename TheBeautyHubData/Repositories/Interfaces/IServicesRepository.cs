using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Services repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IServicesRepository
    {
        Task<Services> InsertServicesAsync(Services services);
        Task<Services> UpdateServicesAsync(Services services);
        Task<int> DeleteServicesAsync(Guid serviceId);
        Task<Services?> GetServicesByIdAsync(Guid serviceId);
        Task<IEnumerable<Services>> GetServicesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Services>> GetAllServicesAsync();
    }
}
