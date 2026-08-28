using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IServicesRepository
    {
        Task<IReadOnlyList<Services>> GetByAccountIdAsync(Guid accountId);
        Task<Services?> GetByIdAsync(Guid serviceId, Guid accountId);
        Task<Services?> GetDetailsByIdAsync(Guid serviceId, Guid accountId);
        Task<Services> InsertAsync(Services service);
        Task UpdateAsync(Services service);
        Task SoftDeleteAsync(Services service);
        Task ReplaceBranchesAsync(Guid serviceId, IEnumerable<Guid> branchIds);
        Task<IReadOnlyList<Branch>> GetBranchesByIdsAsync(Guid accountId, IEnumerable<Guid> branchIds);
        Task RemoveBranchLinksAsync(Guid serviceId);
    }
}
