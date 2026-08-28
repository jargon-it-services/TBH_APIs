using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IServicesService
    {
        Task<IReadOnlyList<ServiceCatalogItemDto>> GetCatalogAsync(Guid accountId);
        Task<IReadOnlyList<ServiceListItemDto>> GetListAsync(Guid accountId);
        Task<ServiceDetailDto?> GetDetailsAsync(Guid serviceId, Guid accountId);
        Task CreateAsync(SaveServiceDto dto);
        Task UpdateAsync(Guid serviceId, SaveServiceDto dto);
        Task DeleteAsync(Guid serviceId, Guid accountId);
    }
}
