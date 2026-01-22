using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IFirmDetailsService
    {
        Task<FirmDetailsDto> CreateFirmDetailsAsync(CreateFirmDetailsDto createFirmDetailsDto);
        Task<FirmDetailsDto> UpdateFirmDetailsAsync(UpdateFirmDetailsDto updateFirmDetailsDto);
        Task<bool> DeleteFirmDetailsAsync(Guid firmDetailsId);
        Task<FirmDetailsDto?> GetFirmDetailsByIdAsync(Guid firmDetailsId);
        Task<IEnumerable<FirmDetailsDto>> GetAllFirmDetailsAsync();
        Task<IEnumerable<FirmDetailsDto>> GetFirmDetailsByFirmIdAsync(Guid firmId);
        Task<IEnumerable<FirmDetailsDto>> GetFirmDetailsByUserIdAsync(Guid userId);
        Task<IEnumerable<FirmDetailsDto>> GetFirmDetailsByAccountIdAsync(Guid accountId);
    }
}
