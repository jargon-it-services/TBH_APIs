using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IPlansService
    {
        Task<PlansDto> CreatePlanAsync(CreatePlanDto createPlanDto);
        Task<PlansDto> UpdatePlanAsync(UpdatePlanDto updatePlanDto);
        Task<bool> DeletePlanAsync(Guid planId);
        Task<PlansDto?> GetPlanByIdAsync(Guid planId);
        Task<IEnumerable<PlansDto>> GetAllPlansAsync();
        Task<IEnumerable<PlansDto>> GetActivePlansAsync();
    }
}
