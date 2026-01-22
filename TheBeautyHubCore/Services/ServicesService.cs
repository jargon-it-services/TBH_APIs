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
    /// <summary>
    /// Service implementation for Services business logic.
    /// Handles validation and business rules for services operations.
    /// </summary>
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly IMapper _mapper;

        public ServicesService(IServicesRepository servicesRepository, IMapper mapper)
        {
            _servicesRepository = servicesRepository;
            _mapper = mapper;
        }

        public async Task<ServicesDto> CreateServicesAsync(CreateServicesDto createServicesDto)
        {
            if (createServicesDto == null)
                throw new ArgumentNullException(nameof(createServicesDto));

            if (string.IsNullOrWhiteSpace(createServicesDto.ServiceName))
                throw new ArgumentException("Service name is required.");

            if (createServicesDto.ServicePrice < 0)
                throw new ArgumentException("Service price cannot be negative.");

            if (createServicesDto.IncentivePercentage.HasValue && 
                (createServicesDto.IncentivePercentage.Value < 0 || createServicesDto.IncentivePercentage.Value > 100))
                throw new ArgumentException("Incentive percentage must be between 0 and 100.");

            var services = _mapper.Map<TheBeautyHubData.Entities.Services>(createServicesDto);
            var createdServices = await _servicesRepository.InsertServicesAsync(services);
            return _mapper.Map<ServicesDto>(createdServices);
        }

        public async Task<ServicesDto> UpdateServicesAsync(UpdateServicesDto updateServicesDto)
        {
            if (updateServicesDto == null)
                throw new ArgumentNullException(nameof(updateServicesDto));

            if (string.IsNullOrWhiteSpace(updateServicesDto.ServiceName))
                throw new ArgumentException("Service name is required.");

            if (updateServicesDto.ServicePrice < 0)
                throw new ArgumentException("Service price cannot be negative.");

            if (updateServicesDto.IncentivePercentage.HasValue && 
                (updateServicesDto.IncentivePercentage.Value < 0 || updateServicesDto.IncentivePercentage.Value > 100))
                throw new ArgumentException("Incentive percentage must be between 0 and 100.");

            var existingServices = await _servicesRepository.GetServicesByIdAsync(updateServicesDto.ServiceId);
            if (existingServices == null)
                throw new KeyNotFoundException($"Service with ID {updateServicesDto.ServiceId} not found.");

            var services = _mapper.Map<TheBeautyHubData.Entities.Services>(updateServicesDto);
            var updatedServices = await _servicesRepository.UpdateServicesAsync(services);
            return _mapper.Map<ServicesDto>(updatedServices);
        }

        public async Task<bool> DeleteServicesAsync(Guid serviceId)
        {
            var existingServices = await _servicesRepository.GetServicesByIdAsync(serviceId);
            if (existingServices == null)
                throw new KeyNotFoundException($"Service with ID {serviceId} not found.");

            var result = await _servicesRepository.DeleteServicesAsync(serviceId);
            return result > 0;
        }

        public async Task<ServicesDto?> GetServicesByIdAsync(Guid serviceId)
        {
            var services = await _servicesRepository.GetServicesByIdAsync(serviceId);
            return services != null ? _mapper.Map<ServicesDto>(services) : null;
        }

        public async Task<IEnumerable<ServicesDto>> GetServicesByAccountIdAsync(Guid accountId)
        {
            var services = await _servicesRepository.GetServicesByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<ServicesDto>>(services);
        }

        public async Task<IEnumerable<ServicesDto>> GetAllServicesAsync()
        {
            var services = await _servicesRepository.GetAllServicesAsync();
            return _mapper.Map<IEnumerable<ServicesDto>>(services);
        }
    }
}
