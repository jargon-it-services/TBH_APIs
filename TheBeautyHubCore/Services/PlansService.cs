using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class PlansService : IPlansService
    {
        private readonly IPlansRepository _plansRepository;
        private readonly IMapper _mapper;

        public PlansService(IPlansRepository plansRepository, IMapper mapper)
        {
            _plansRepository = plansRepository;
            _mapper = mapper;
        }

        public async Task<PlansDto> CreatePlanAsync(CreatePlanDto createPlanDto)
        {
            if (string.IsNullOrWhiteSpace(createPlanDto.PlanName))
                throw new ArgumentException("Plan name is required.");

            if (createPlanDto.PlanCost < 0)
                throw new ArgumentException("Plan cost cannot be negative.");

            var plan = _mapper.Map<Plans>(createPlanDto);
            plan.CreatedAt = DateTime.UtcNow;

            var insertedPlan = await _plansRepository.InsertPlanAsync(plan);
            return _mapper.Map<PlansDto>(insertedPlan);
        }

        public async Task<PlansDto> UpdatePlanAsync(UpdatePlanDto updatePlanDto)
        {
            if (string.IsNullOrWhiteSpace(updatePlanDto.PlanName))
                throw new ArgumentException("Plan name is required.");

            if (updatePlanDto.PlanCost < 0)
                throw new ArgumentException("Plan cost cannot be negative.");

            var existingPlan = await _plansRepository.GetPlanByIdAsync(updatePlanDto.PlanId);
            if (existingPlan == null)
                throw new KeyNotFoundException($"Plan with ID {updatePlanDto.PlanId} not found.");

            var plan = _mapper.Map<Plans>(updatePlanDto);
            plan.CreatedAt = existingPlan.CreatedAt;

            var updatedPlan = await _plansRepository.UpdatePlanAsync(plan);
            return _mapper.Map<PlansDto>(updatedPlan);
        }

        public async Task<bool> DeletePlanAsync(Guid planId)
        {
            var existingPlan = await _plansRepository.GetPlanByIdAsync(planId);
            if (existingPlan == null)
                throw new KeyNotFoundException($"Plan with ID {planId} not found.");

            var result = await _plansRepository.DeletePlanAsync(planId);
            return result > 0;
        }

        public async Task<PlansDto?> GetPlanByIdAsync(Guid planId)
        {
            var plan = await _plansRepository.GetPlanByIdAsync(planId);
            return plan == null ? null : _mapper.Map<PlansDto>(plan);
        }

        public async Task<IEnumerable<PlansDto>> GetAllPlansAsync()
        {
            var plans = await _plansRepository.GetAllPlansAsync();
            return _mapper.Map<IEnumerable<PlansDto>>(plans);
        }

        public async Task<IEnumerable<PlansDto>> GetActivePlansAsync()
        {
            var plans = await _plansRepository.GetActivePlansAsync();
            return _mapper.Map<IEnumerable<PlansDto>>(plans);
        }
    }
}
