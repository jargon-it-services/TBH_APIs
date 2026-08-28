using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<IEnumerable<BranchListItemDto>> GetBranchesAsync(Guid accountId)
        {
            var branches = await _branchRepository.GetAllAsync(accountId);
            return branches.Select(MapListItem).ToList();
        }

        public async Task<BranchDetailDto?> GetBranchDetailsAsync(Guid branchId, Guid accountId)
        {
            var branch = await _branchRepository.GetDetailsByIdAsync(branchId);
            if (branch == null || !BelongsToAccount(branch, accountId))
                return null;

            return MapDetail(branch);
        }

        public async Task<BranchSavedDto> CreateBranchAsync(SaveBranchDto dto)
        {
            ValidateWrite(dto);
            if (!dto.AccountId.HasValue || dto.AccountId == Guid.Empty)
                throw new ArgumentException("Authenticated account is required.");

            var serviceIds = await ResolveServiceIdsAsync(dto.Services);

            var branch = new Branch
            {
                AccountId = dto.AccountId,
                Name = dto.Name.Trim(),
                AddressLine1 = dto.AddressLine1.Trim(),
                AddressLine2 = string.IsNullOrWhiteSpace(dto.AddressLine2) ? null : dto.AddressLine2.Trim(),
                City = dto.City.Trim(),
                State = dto.State.Trim(),
                Pincode = dto.Pincode.Trim(),
                Mobile = dto.Mobile.Trim(),
                Email = dto.Email.Trim(),
                BranchType = dto.BranchType.Trim(),
                OpeningTime = dto.OpeningTime.Trim(),
                ClosingTime = dto.ClosingTime.Trim(),
                WeeklyOff = dto.WeeklyOff.Trim(),
                Status = dto.Status.Trim(),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                MapsLink = string.IsNullOrWhiteSpace(dto.MapsLink) ? null : dto.MapsLink.Trim(),
                Logo = dto.Logo,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var inserted = await _branchRepository.InsertAsync(branch);
            await _branchRepository.ReplaceServicesAsync(inserted.BranchId, serviceIds);

            return new BranchSavedDto { Saved = true };
        }

        public async Task<BranchSavedDto> UpdateBranchAsync(Guid branchId, Guid accountId, SaveBranchDto dto)
        {
            ValidateWrite(dto);

            var existing = await _branchRepository.GetByIdAsync(branchId);
            if (existing == null || !BelongsToAccount(existing, accountId))
                throw new KeyNotFoundException($"Branch with ID {branchId} not found.");

            var serviceIds = await ResolveServiceIdsAsync(dto.Services);

            existing.Name = dto.Name.Trim();
            existing.AddressLine1 = dto.AddressLine1.Trim();
            existing.AddressLine2 = string.IsNullOrWhiteSpace(dto.AddressLine2) ? null : dto.AddressLine2.Trim();
            existing.City = dto.City.Trim();
            existing.State = dto.State.Trim();
            existing.Pincode = dto.Pincode.Trim();
            existing.Mobile = dto.Mobile.Trim();
            existing.Email = dto.Email.Trim();
            existing.BranchType = dto.BranchType.Trim();
            existing.OpeningTime = dto.OpeningTime.Trim();
            existing.ClosingTime = dto.ClosingTime.Trim();
            existing.WeeklyOff = dto.WeeklyOff.Trim();
            existing.Status = dto.Status.Trim();
            existing.Latitude = dto.Latitude;
            existing.Longitude = dto.Longitude;
            existing.MapsLink = string.IsNullOrWhiteSpace(dto.MapsLink) ? null : dto.MapsLink.Trim();

            if (dto.AccountId.HasValue)
                existing.AccountId = dto.AccountId;
            else
                existing.AccountId = accountId;

            if (dto.RemoveLogo)
                existing.Logo = null;
            else if (dto.HasNewLogo)
                existing.Logo = dto.Logo;

            await _branchRepository.UpdateAsync(existing);
            if (dto.Services != null)
                await _branchRepository.ReplaceServicesAsync(existing.BranchId, serviceIds);

            return new BranchSavedDto { Saved = true };
        }

        private async Task<List<Guid>> ResolveServiceIdsAsync(IEnumerable<Guid>? serviceIds)
        {
            var requested = (serviceIds ?? Enumerable.Empty<Guid>()).Distinct().ToList();
            if (requested.Count == 0)
                return requested;

            var found = await _branchRepository.GetServicesByIdsAsync(requested);
            if (found.Count != requested.Count)
                throw new ArgumentException("One or more service IDs are invalid.");

            return requested;
        }

        private static void ValidateWrite(SaveBranchDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("name is required.");
            if (string.IsNullOrWhiteSpace(dto.AddressLine1))
                throw new ArgumentException("address_line1 is required.");
            if (string.IsNullOrWhiteSpace(dto.City))
                throw new ArgumentException("city is required.");
            if (string.IsNullOrWhiteSpace(dto.State))
                throw new ArgumentException("state is required.");
            if (string.IsNullOrWhiteSpace(dto.Pincode))
                throw new ArgumentException("pincode is required.");
            if (string.IsNullOrWhiteSpace(dto.Mobile))
                throw new ArgumentException("mobile is required.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("email is required.");
            if (string.IsNullOrWhiteSpace(dto.BranchType))
                throw new ArgumentException("branch_type is required.");
            if (string.IsNullOrWhiteSpace(dto.OpeningTime))
                throw new ArgumentException("opening_time is required.");
            if (string.IsNullOrWhiteSpace(dto.ClosingTime))
                throw new ArgumentException("closing_time is required.");
            if (string.IsNullOrWhiteSpace(dto.WeeklyOff))
                throw new ArgumentException("weekly_off is required.");
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("status is required.");
        }

        private static bool BelongsToAccount(Branch branch, Guid accountId)
        {
            return branch.AccountId == accountId;
        }

        private static BranchListItemDto MapListItem(Branch branch)
        {
            var address = string.IsNullOrWhiteSpace(branch.AddressLine2)
                ? branch.AddressLine1
                : $"{branch.AddressLine1}, {branch.AddressLine2}";

            return new BranchListItemDto
            {
                Id = branch.BranchId,
                Name = branch.Name,
                Address = address,
                City = branch.City,
                State = branch.State,
                Mobile = branch.Mobile,
                BranchType = branch.BranchType,
                Status = branch.Status,
                Logo = branch.Logo
            };
        }

        private static BranchDetailDto MapDetail(Branch branch)
        {
            return new BranchDetailDto
            {
                Id = branch.BranchId,
                Name = branch.Name,
                AddressLine1 = branch.AddressLine1,
                AddressLine2 = branch.AddressLine2 ?? string.Empty,
                City = branch.City,
                State = branch.State,
                Pincode = branch.Pincode,
                Mobile = branch.Mobile,
                Email = branch.Email,
                BranchType = branch.BranchType,
                OpeningTime = branch.OpeningTime,
                ClosingTime = branch.ClosingTime,
                WeeklyOff = branch.WeeklyOff,
                Status = branch.Status,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude,
                MapsLink = branch.MapsLink,
                Logo = branch.Logo,
                Services = branch.BranchServices
                    .Where(bs => bs.Service != null && !bs.Service.IsDeleted)
                    .Select(bs => new BranchServiceItemDto
                    {
                        Id = bs.ServiceId,
                        Name = bs.Service.ServiceName
                    })
                    .ToList(),
                Employees = branch.BranchEmployees
                    .Where(be => be.User != null && !be.User.IsDeleted)
                    .Select(be => new BranchEmployeeItemDto
                    {
                        Id = be.UserId,
                        Name = be.User.UserName,
                        Role = be.User.UserRole,
                        Photo = be.Photo
                    })
                    .ToList()
            };
        }
    }
}
