using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IFirmService
    {
        Task<FirmDto> CreateFirmAsync(CreateFirmDto createFirmDto);
        Task<FirmDto> UpdateFirmAsync(UpdateFirmDto updateFirmDto);
        Task<bool> DeleteFirmAsync(Guid firmId);
        Task<FirmDto?> GetFirmByIdAsync(Guid firmId);
        Task<IEnumerable<FirmDto>> GetAllFirmsAsync();
        Task<IEnumerable<FirmDto>> GetFirmsByAccountIdAsync(Guid accountId);
    }
}
