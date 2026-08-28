using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetAllAsync(Guid accountId);
        Task<Branch?> GetByIdAsync(Guid branchId);
        Task<Branch?> GetDetailsByIdAsync(Guid branchId);
        Task<Branch> InsertAsync(Branch branch);
        Task<Branch> UpdateAsync(Branch branch);
        Task ReplaceServicesAsync(Guid branchId, IEnumerable<Guid> serviceIds);
        Task<IReadOnlyList<Services>> GetServicesByIdsAsync(IEnumerable<Guid> serviceIds);
    }
}
