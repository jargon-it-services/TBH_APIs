using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IStaffService
    {
        Task<StaffFormConfigDto> GetFormConfigAsync(Guid accountId);
        Task<IReadOnlyList<StaffListItemDto>> GetListAsync(Guid accountId);
        Task<StaffDetailDto?> GetDetailsAsync(Guid staffId, Guid accountId);
        Task<string?> GetNextEmployeeCodeAsync(Guid accountId);
        Task CreateAsync(SaveStaffDto dto);
        Task UpdateAsync(Guid staffId, SaveStaffDto dto);
        Task UpdateStatusAsync(Guid staffId, Guid accountId, string status);
        Task DeleteAsync(Guid staffId, Guid accountId);
    }
}
