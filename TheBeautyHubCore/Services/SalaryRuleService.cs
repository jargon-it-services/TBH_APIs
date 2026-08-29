using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class SalaryRuleService : ISalaryRuleService
    {
        private readonly IStaffRepository _staffRepository;

        public SalaryRuleService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IReadOnlyList<SalaryRuleCatalogItemDto>> GetCatalogAsync(Guid accountId)
        {
            var rules = await _staffRepository.GetSalaryRulesAsync(accountId);
            return rules.Select(r => new SalaryRuleCatalogItemDto
            {
                Id = r.SalaryRuleId,
                Name = r.Name,
                Active = string.Equals(r.Status, "active", StringComparison.OrdinalIgnoreCase) || r.IsActive
            }).ToList();
        }

        public async Task<IReadOnlyList<SalaryRuleListItemDto>> GetListAsync(Guid accountId)
        {
            var rules = await _staffRepository.GetSalaryRulesAsync(accountId);
            return rules.Select(r => new SalaryRuleListItemDto
            {
                Id = r.SalaryRuleId,
                Name = r.Name,
                SalaryType = r.SalaryType,
                FixedSalary = r.FixedSalary,
                Status = r.Status
            }).ToList();
        }

        public async Task<SalaryRuleDetailDto?> GetDetailsAsync(Guid ruleId, Guid accountId)
        {
            var rule = await _staffRepository.GetSalaryRuleAsync(ruleId, accountId);
            return rule == null ? null : MapDetail(rule);
        }

        public async Task CreateAsync(SaveSalaryRuleDto dto)
        {
            ValidateWrite(dto);
            var rule = new SalaryRule
            {
                AccountId = dto.AccountId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            ApplyFields(rule, dto);
            await _staffRepository.InsertSalaryRuleAsync(rule);
        }

        public async Task UpdateAsync(Guid ruleId, SaveSalaryRuleDto dto)
        {
            ValidateWrite(dto);
            var existing = await _staffRepository.GetSalaryRuleAsync(ruleId, dto.AccountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.SalaryRuleNotFound);

            ApplyFields(existing, dto);
            await _staffRepository.UpdateSalaryRuleAsync(existing);
        }

        public async Task DeleteAsync(Guid ruleId, Guid accountId)
        {
            var existing = await _staffRepository.GetSalaryRuleAsync(ruleId, accountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.SalaryRuleNotFound);

            await _staffRepository.SoftDeleteSalaryRuleAsync(existing);
        }

        private static void ApplyFields(SalaryRule rule, SaveSalaryRuleDto dto)
        {
            var status = dto.Status.Trim();
            rule.Name = dto.Name.Trim();
            rule.Description = dto.Description.Trim();
            rule.SalaryType = dto.SalaryType.Trim();
            rule.FixedSalary = dto.FixedSalary;
            rule.MonthlyTarget = dto.MonthlyTarget;
            rule.TargetBonus = dto.TargetBonus;
            rule.AllowAdvanceRecovery = dto.AllowAdvanceRecovery;
            rule.MaxRecoveryPerMonth = dto.MaxRecoveryPerMonth;
            rule.Status = status;
            rule.IsActive = string.Equals(status, "active", StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidateWrite(SaveSalaryRuleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(ApiMessages.SalaryRuleNameRequired);
            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException(ApiMessages.SalaryRuleDescriptionRequired);
            if (string.IsNullOrWhiteSpace(dto.SalaryType))
                throw new ArgumentException(ApiMessages.SalaryRuleTypeRequired);
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException(ApiMessages.SalaryRuleStatusRequired);
        }

        private static SalaryRuleDetailDto MapDetail(SalaryRule rule)
        {
            return new SalaryRuleDetailDto
            {
                Id = rule.SalaryRuleId,
                Name = rule.Name,
                Description = rule.Description,
                SalaryType = rule.SalaryType,
                FixedSalary = rule.FixedSalary,
                MonthlyTarget = rule.MonthlyTarget,
                TargetBonus = rule.TargetBonus,
                AllowAdvanceRecovery = rule.AllowAdvanceRecovery,
                MaxRecoveryPerMonth = rule.MaxRecoveryPerMonth,
                Status = rule.Status
            };
        }
    }
}
