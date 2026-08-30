using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface ISalaryRuleService
    {
        Task<IReadOnlyList<SalaryRuleCatalogItemDto>> GetCatalogAsync(Guid accountId);
        Task<IReadOnlyList<SalaryRuleListItemDto>> GetListAsync(Guid accountId);
        Task<SalaryRuleDetailDto?> GetDetailsAsync(Guid ruleId, Guid accountId);
        Task CreateAsync(SaveSalaryRuleDto dto);
        Task UpdateAsync(Guid ruleId, SaveSalaryRuleDto dto);
        Task UpdateStatusAsync(Guid ruleId, Guid accountId, string status);
        Task DeleteAsync(Guid ruleId, Guid accountId);
    }
}
