using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubData.Enums;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Repositories.Interfaces;
using ServiceEntity = TheBeautyHubData.Entities.Services;

namespace TheBeautyHubCore.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _servicesRepository;

        public ServicesService(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public async Task<IReadOnlyList<ServiceCatalogItemDto>> GetCatalogAsync(Guid accountId)
        {
            var services = await _servicesRepository.GetByAccountIdAsync(accountId);
            return services.Select(s => new ServiceCatalogItemDto
            {
                Id = s.ServiceId,
                Name = s.ServiceName,
                Active = RecordStatuses.IsActive(s.Status)
            }).ToList();
        }

        public async Task<IReadOnlyList<ServiceListItemDto>> GetListAsync(Guid accountId)
        {
            var services = await _servicesRepository.GetByAccountIdAsync(accountId);
            return services.Select(s => new ServiceListItemDto
            {
                Id = s.ServiceId,
                Name = s.ServiceName,
                Category = s.Category,
                ApplicableGender = s.ApplicableGender,
                DurationMinutes = s.DurationMinutes,
                CustomerPrice = s.ServicePrice,
                Status = s.Status,
                Type = s.OfferingType,
                Photo = s.Photo
            }).ToList();
        }

        public async Task<ServiceDetailDto?> GetDetailsAsync(Guid serviceId, Guid accountId)
        {
            var service = await _servicesRepository.GetDetailsByIdAsync(serviceId, accountId);
            return service == null ? null : MapDetail(service);
        }

        public async Task CreateAsync(SaveServiceDto dto)
        {
            ValidateWrite(dto);
            var branchIds = await ResolveBranchIdsAsync(dto);

            var service = new ServiceEntity
            {
                AccountId = dto.AccountId,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            ApplyFields(service, dto);

            var inserted = await _servicesRepository.InsertAsync(service);
            await _servicesRepository.ReplaceBranchesAsync(inserted.ServiceId, branchIds);
        }

        public async Task UpdateAsync(Guid serviceId, SaveServiceDto dto)
        {
            ValidateWrite(dto);

            var existing = await _servicesRepository.GetByIdAsync(serviceId, dto.AccountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.ServiceNotFound);

            var branchIds = await ResolveBranchIdsAsync(dto);
            ApplyFields(existing, dto);

            if (dto.RemovePhoto)
                existing.Photo = null;
            else if (dto.HasNewPhoto)
                existing.Photo = dto.Photo;

            await _servicesRepository.UpdateAsync(existing);
            await _servicesRepository.ReplaceBranchesAsync(existing.ServiceId, branchIds);
        }

        public async Task UpdateStatusAsync(Guid serviceId, Guid accountId, string status)
        {
            var existing = await _servicesRepository.GetByIdAsync(serviceId, accountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.ServiceNotFound);

            existing.Status = RecordStatuses.ParseOrThrow(status, ApiMessages.RecordStatusInvalid).ToApiValue();
            await _servicesRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid serviceId, Guid accountId)
        {
            var existing = await _servicesRepository.GetByIdAsync(serviceId, accountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.ServiceNotFound);

            await _servicesRepository.RemoveBranchLinksAsync(serviceId);
            await _servicesRepository.SoftDeleteAsync(existing);
        }

        private async Task<List<Guid>> ResolveBranchIdsAsync(SaveServiceDto dto)
        {
            if (dto.AllBranches)
                return new List<Guid>();

            var requested = (dto.Branches ?? new List<Guid>()).Distinct().ToList();
            if (requested.Count == 0)
                throw new ArgumentException(ApiMessages.BranchesRequiredWhenNotAll);

            var found = await _servicesRepository.GetBranchesByIdsAsync(dto.AccountId, requested);
            if (found.Count != requested.Count)
                throw new ArgumentException(ApiMessages.InvalidBranchIds);

            return requested;
        }

        private static void ApplyFields(ServiceEntity service, SaveServiceDto dto)
        {
            var commissionType = CommissionTypes.ParseOrThrow(dto.CommissionType, ApiMessages.ServiceCommissionTypeInvalid);

            service.ServiceName = dto.Name.Trim();
            service.ServiceDescription = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            service.ServicePrice = dto.CustomerPrice;
            service.Category = dto.Category.Trim();
            service.DurationMinutes = dto.DurationMinutes;
            service.ApplicableGender = ServiceGenders.ParseOrThrow(dto.ApplicableGender, ApiMessages.ServiceGenderInvalid).ToApiValue();
            service.OfferingType = ServiceOfferingTypes.ParseOrThrow(dto.Type, ApiMessages.ServiceTypeInvalid).ToApiValue();
            service.Status = RecordStatuses.ParseOrThrow(dto.Status, ApiMessages.RecordStatusInvalid).ToApiValue();
            service.MaterialCost = dto.MaterialCost;
            service.CommissionType = commissionType.ToApiValue();
            service.CommissionValue = dto.CommissionValue;
            service.OtherCost = dto.OtherCost;
            service.HomeServiceAvailable = dto.HomeServiceAvailable;
            service.HomeVisitCharges = dto.HomeVisitCharges;
            service.ServiceRadiusKm = dto.ServiceRadiusKm;
            service.ExtraChargePerKm = dto.ExtraChargePerKm;
            service.AllBranches = dto.AllBranches;
            if (dto.HasNewPhoto)
                service.Photo = dto.Photo;

            service.IsIncentiveApplicable = dto.CommissionValue > 0;
            if (commissionType == CommissionType.Percentage)
            {
                service.IncentivePercentage = dto.CommissionValue;
                service.IncentiveAmount = null;
            }
            else
            {
                service.IncentiveAmount = dto.CommissionValue;
                service.IncentivePercentage = null;
            }
        }

        private static void ValidateWrite(SaveServiceDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(ApiMessages.ServiceNameRequired);
            if (string.IsNullOrWhiteSpace(dto.Category))
                throw new ArgumentException(ApiMessages.ServiceCategoryRequired);
            if (dto.DurationMinutes < 0)
                throw new ArgumentException(ApiMessages.ServiceDurationInvalid);
            if (string.IsNullOrWhiteSpace(dto.ApplicableGender))
                throw new ArgumentException(ApiMessages.ServiceGenderRequired);
            _ = ServiceGenders.ParseOrThrow(dto.ApplicableGender, ApiMessages.ServiceGenderInvalid);
            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new ArgumentException(ApiMessages.ServiceTypeRequired);
            _ = ServiceOfferingTypes.ParseOrThrow(dto.Type, ApiMessages.ServiceTypeInvalid);
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException(ApiMessages.ServiceStatusRequired);
            _ = RecordStatuses.ParseOrThrow(dto.Status, ApiMessages.RecordStatusInvalid);
            if (dto.CustomerPrice < 0)
                throw new ArgumentException(ApiMessages.ServiceCustomerPriceInvalid);
            if (dto.MaterialCost < 0)
                throw new ArgumentException(ApiMessages.ServiceMaterialCostInvalid);
            if (string.IsNullOrWhiteSpace(dto.CommissionType))
                throw new ArgumentException(ApiMessages.ServiceCommissionTypeRequired);

            var commissionType = CommissionTypes.ParseOrThrow(dto.CommissionType, ApiMessages.ServiceCommissionTypeInvalid);
            if (commissionType == CommissionType.Percentage && (dto.CommissionValue < 0 || dto.CommissionValue > 100))
                throw new ArgumentException(ApiMessages.ServiceCommissionPercentageInvalid);
            if (dto.CommissionValue < 0)
                throw new ArgumentException(ApiMessages.ServiceCommissionValueInvalid);
            if (dto.OtherCost < 0)
                throw new ArgumentException(ApiMessages.ServiceOtherCostInvalid);
        }

        private static ServiceDetailDto MapDetail(ServiceEntity service)
        {
            var branches = service.AllBranches
                ? new List<ServiceBranchItemDto>()
                : service.BranchServices
                    .Where(bs => bs.Branch != null && !bs.Branch.IsDeleted)
                    .Select(bs => new ServiceBranchItemDto
                    {
                        Id = bs.BranchId,
                        Name = bs.Branch.Name
                    })
                    .OrderBy(b => b.Name)
                    .ToList();

            return new ServiceDetailDto
            {
                Id = service.ServiceId,
                Name = service.ServiceName,
                Description = service.ServiceDescription ?? string.Empty,
                Category = service.Category,
                DurationMinutes = service.DurationMinutes,
                ApplicableGender = service.ApplicableGender,
                Type = service.OfferingType,
                Status = service.Status,
                CustomerPrice = service.ServicePrice,
                MaterialCost = service.MaterialCost,
                CommissionType = MapCommissionTypeForResponse(service.CommissionType),
                CommissionValue = service.CommissionValue,
                OtherCost = service.OtherCost,
                HomeServiceAvailable = service.HomeServiceAvailable,
                HomeVisitCharges = service.HomeVisitCharges,
                ServiceRadiusKm = service.ServiceRadiusKm,
                ExtraChargePerKm = service.ExtraChargePerKm,
                AllBranches = service.AllBranches,
                Branches = branches,
                Photo = service.Photo
            };
        }

        private static string MapCommissionTypeForResponse(string? stored)
        {
            return CommissionTypes.TryParse(stored, out var type)
                ? type.ToApiValue()
                : stored ?? string.Empty;
        }
    }
}
