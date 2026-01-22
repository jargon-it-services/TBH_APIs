using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Service interface for Services operations.
    /// Defines business logic layer contract for services management.
    /// </summary>
    public interface IServicesService
    {
        Task<ServicesDto> CreateServicesAsync(CreateServicesDto createServicesDto);
        Task<ServicesDto> UpdateServicesAsync(UpdateServicesDto updateServicesDto);
        Task<bool> DeleteServicesAsync(Guid serviceId);
        Task<ServicesDto?> GetServicesByIdAsync(Guid serviceId);
        Task<IEnumerable<ServicesDto>> GetServicesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<ServicesDto>> GetAllServicesAsync();
    }
}
