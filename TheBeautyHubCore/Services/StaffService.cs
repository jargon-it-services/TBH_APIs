using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class StaffService : IStaffService
    {
        private static readonly string[] DefaultSpecialists = { "Hair", "Skin", "Nails", "Massage" };
        private static readonly Regex EmployeeCodePattern = new(@"^EMP(\d+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IStaffRepository _staffRepository;
        private readonly IBranchRepository _branchRepository;

        public StaffService(
            IStaffRepository staffRepository,
            IBranchRepository branchRepository)
        {
            _staffRepository = staffRepository;
            _branchRepository = branchRepository;
        }

        public async Task<StaffFormConfigDto> GetFormConfigAsync(Guid accountId)
        {
            await _staffRepository.EnsureDefaultSalaryRulesAsync(accountId);

            var branches = await _branchRepository.GetAllAsync(accountId);
            var rules = await _staffRepository.GetSalaryRulesAsync(accountId);
            var fromStaff = await _staffRepository.GetSpecialistsAsync(accountId);

            var specialists = DefaultSpecialists
                .Concat(fromStaff)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s)
                .ToList();

            return new StaffFormConfigDto
            {
                Branches = branches.Select(b => new StaffFormBranchDto
                {
                    Id = b.BranchId,
                    Name = b.Name,
                    Code = string.IsNullOrWhiteSpace(b.Code) ? DeriveBranchCode(b.Name) : b.Code
                }).ToList(),
                SalaryRules = rules.Select(r => new StaffFormSalaryRuleDto
                {
                    Id = r.SalaryRuleId,
                    Name = r.Name,
                    Active = string.Equals(r.Status, "active", StringComparison.OrdinalIgnoreCase) || r.IsActive
                }).ToList(),
                Specialists = specialists
            };
        }

        public async Task<IReadOnlyList<StaffListItemDto>> GetListAsync(Guid accountId)
        {
            var staff = await _staffRepository.GetAllAsync(accountId);
            return staff.Select(s => new StaffListItemDto
            {
                Id = s.StaffId,
                FullName = s.FullName,
                Mobile = s.Mobile,
                Email = s.Email,
                EmployeeCode = s.EmployeeCode ?? string.Empty,
                Designation = s.Designation,
                Specialist = s.Specialist,
                BranchName = s.Branch?.Name ?? string.Empty,
                Status = s.Status,
                Photo = s.Photo
            }).ToList();
        }

        public async Task<StaffDetailDto?> GetDetailsAsync(Guid staffId, Guid accountId)
        {
            var staff = await _staffRepository.GetByIdAsync(staffId, accountId);
            if (staff == null)
                return null;

            return new StaffDetailDto
            {
                Id = staff.StaffId,
                FullName = staff.FullName,
                Mobile = staff.Mobile,
                Email = staff.Email,
                Gender = staff.Gender,
                AadhaarNumber = staff.AadhaarNumber,
                EmployeeCode = staff.EmployeeCode ?? string.Empty,
                JoiningDate = staff.JoiningDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                Designation = staff.Designation,
                Specialist = staff.Specialist,
                BranchId = staff.BranchId,
                BranchName = staff.Branch?.Name ?? string.Empty,
                SalaryRuleId = staff.SalaryRuleId,
                SalaryRuleName = staff.SalaryRule?.Name ?? string.Empty,
                Status = staff.Status,
                AllowAppLogin = staff.AllowAppLogin,
                AppRole = staff.AppRole ?? string.Empty,
                Username = staff.Username ?? string.Empty,
                Photo = staff.Photo,
                AadhaarCardUrl = staff.AadhaarCardUrl
            };
        }

        public async Task<string?> GetNextEmployeeCodeAsync(Guid accountId)
        {
            var codes = await _staffRepository.GetEmployeeCodesAsync(accountId);
            var max = 0;
            foreach (var code in codes)
            {
                var match = EmployeeCodePattern.Match(code.Trim());
                if (match.Success && int.TryParse(match.Groups[1].Value, out var number) && number > max)
                    max = number;
            }

            return $"EMP{(max + 1).ToString("D3", CultureInfo.InvariantCulture)}";
        }

        public async Task CreateAsync(SaveStaffDto dto)
        {
            ValidateWrite(dto, isUpdate: false);
            await EnsureBranchAndRuleAsync(dto);

            if (!string.IsNullOrWhiteSpace(dto.EmployeeCode) &&
                await _staffRepository.EmployeeCodeExistsAsync(dto.AccountId, dto.EmployeeCode.Trim()))
            {
                throw new ArgumentException(ApiMessages.StaffEmployeeCodeExists);
            }

            var staff = new Staff
            {
                AccountId = dto.AccountId,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            ApplyFields(staff, dto);
            if (dto.HasNewPhoto)
                staff.Photo = dto.Photo;
            if (dto.HasNewAadhaarCard)
                staff.AadhaarCardUrl = dto.AadhaarCardUrl;

            if (dto.AllowAppLogin && dto.UserId.HasValue && dto.UserId.Value != Guid.Empty)
                staff.UserId = dto.UserId;

            var inserted = await _staffRepository.InsertAsync(staff);
            if (inserted.UserId.HasValue)
                await _staffRepository.AssignBranchEmployeeAsync(inserted.UserId.Value, inserted.BranchId, inserted.Photo);
        }

        public async Task UpdateAsync(Guid staffId, SaveStaffDto dto)
        {
            ValidateWrite(dto, isUpdate: true);

            var existing = await _staffRepository.GetByIdAsync(staffId, dto.AccountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.StaffNotFound);

            await EnsureBranchAndRuleAsync(dto);

            if (!string.IsNullOrWhiteSpace(dto.EmployeeCode) &&
                await _staffRepository.EmployeeCodeExistsAsync(dto.AccountId, dto.EmployeeCode.Trim(), staffId))
            {
                throw new ArgumentException(ApiMessages.StaffEmployeeCodeExists);
            }

            ApplyFields(existing, dto);

            if (dto.RemovePhoto)
                existing.Photo = null;
            else if (dto.HasNewPhoto)
                existing.Photo = dto.Photo;

            if (dto.RemoveAadhaarCard)
                existing.AadhaarCardUrl = null;
            else if (dto.HasNewAadhaarCard)
                existing.AadhaarCardUrl = dto.AadhaarCardUrl;

            if (dto.AllowAppLogin && dto.UserId.HasValue && dto.UserId.Value != Guid.Empty)
                existing.UserId = dto.UserId;

            await _staffRepository.UpdateAsync(existing);
            if (existing.UserId.HasValue)
                await _staffRepository.AssignBranchEmployeeAsync(existing.UserId.Value, existing.BranchId, existing.Photo);
        }

        public async Task DeleteAsync(Guid staffId, Guid accountId)
        {
            var existing = await _staffRepository.GetByIdAsync(staffId, accountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.StaffNotFound);

            if (existing.UserId.HasValue && existing.UserId.Value != existing.StaffId)
                await _staffRepository.RemoveBranchEmployeesForUserAsync(existing.UserId.Value);

            await _staffRepository.RemoveBranchEmployeesForUserAsync(existing.StaffId);
            await _staffRepository.SoftDeleteAsync(existing);
        }

        private async Task EnsureBranchAndRuleAsync(SaveStaffDto dto)
        {
            var branch = await _branchRepository.GetByIdAsync(dto.BranchId);
            if (branch == null || branch.AccountId != dto.AccountId)
                throw new ArgumentException(ApiMessages.StaffBranchInvalid);

            var rule = await _staffRepository.GetSalaryRuleAsync(dto.SalaryRuleId, dto.AccountId);
            if (rule == null)
                throw new ArgumentException(ApiMessages.StaffSalaryRuleInvalid);
        }

        private static void ApplyFields(Staff staff, SaveStaffDto dto)
        {
            staff.FullName = dto.FullName.Trim();
            staff.Mobile = dto.Mobile.Trim();
            staff.Email = dto.Email.Trim();
            staff.Gender = dto.Gender.Trim();
            staff.AadhaarNumber = dto.AadhaarNumber.Trim();
            staff.EmployeeCode = string.IsNullOrWhiteSpace(dto.EmployeeCode) ? null : dto.EmployeeCode.Trim();
            staff.JoiningDate = ParseJoiningDate(dto.JoiningDate);
            staff.Designation = dto.Designation.Trim();
            staff.Specialist = dto.Specialist.Trim();
            staff.BranchId = dto.BranchId;
            staff.SalaryRuleId = dto.SalaryRuleId;
            staff.Status = dto.Status.Trim();
            staff.AllowAppLogin = dto.AllowAppLogin;
            staff.AppRole = dto.AllowAppLogin ? dto.AppRole?.Trim() : dto.AppRole?.Trim();
            staff.Username = dto.AllowAppLogin ? dto.Username?.Trim() : dto.Username?.Trim();
        }

        private static void ValidateWrite(SaveStaffDto dto, bool isUpdate)
        {
            if (dto.AccountId == Guid.Empty)
                throw new ArgumentException(ApiMessages.AccountRequired);
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException(ApiMessages.StaffFullNameRequired);
            if (string.IsNullOrWhiteSpace(dto.Mobile))
                throw new ArgumentException(ApiMessages.StaffMobileRequired);
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException(ApiMessages.StaffEmailRequired);
            if (string.IsNullOrWhiteSpace(dto.Gender))
                throw new ArgumentException(ApiMessages.StaffGenderRequired);
            if (string.IsNullOrWhiteSpace(dto.AadhaarNumber))
                throw new ArgumentException(ApiMessages.StaffAadhaarRequired);
            if (string.IsNullOrWhiteSpace(dto.Designation))
                throw new ArgumentException(ApiMessages.StaffDesignationRequired);
            if (string.IsNullOrWhiteSpace(dto.Specialist))
                throw new ArgumentException(ApiMessages.StaffSpecialistRequired);
            if (dto.BranchId == Guid.Empty)
                throw new ArgumentException(ApiMessages.StaffBranchRequired);
            if (dto.SalaryRuleId == Guid.Empty)
                throw new ArgumentException(ApiMessages.StaffSalaryRuleRequired);
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException(ApiMessages.StaffStatusRequired);

            if (dto.AllowAppLogin)
            {
                if (string.IsNullOrWhiteSpace(dto.AppRole))
                    throw new ArgumentException(ApiMessages.StaffAppRoleRequired);
                if (!isUpdate && string.IsNullOrWhiteSpace(dto.Username))
                    throw new ArgumentException(ApiMessages.StaffUsernameRequired);
            }
        }

        private static DateTime? ParseJoiningDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
                return DateTime.SpecifyKind(parsed.Date, DateTimeKind.Utc);
            throw new ArgumentException(ApiMessages.StaffJoiningDateInvalid);
        }

        private static string DeriveBranchCode(string name)
        {
            var letters = new string(name.Where(char.IsLetterOrDigit).Take(4).Select(char.ToUpperInvariant).ToArray());
            return string.IsNullOrWhiteSpace(letters) ? "BR01" : letters;
        }
    }
}
