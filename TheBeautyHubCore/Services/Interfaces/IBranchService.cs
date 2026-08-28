using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchListItemDto>> GetBranchesAsync(Guid accountId);
        Task<BranchDetailDto?> GetBranchDetailsAsync(Guid branchId, Guid accountId);
        Task<BranchSavedDto> CreateBranchAsync(SaveBranchDto dto);
        Task<BranchSavedDto> UpdateBranchAsync(Guid branchId, Guid accountId, SaveBranchDto dto);
    }
}
