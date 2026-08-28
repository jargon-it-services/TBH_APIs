using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        Task<IReadOnlyList<Staff>> GetAllAsync(Guid accountId);
        Task<Staff?> GetByIdAsync(Guid staffId, Guid accountId);
        Task<Staff> InsertAsync(Staff staff);
        Task<Staff> UpdateAsync(Staff staff);
        Task SoftDeleteAsync(Staff staff);
        Task<IReadOnlyList<SalaryRule>> GetSalaryRulesAsync(Guid accountId);
        Task EnsureDefaultSalaryRulesAsync(Guid accountId);
        Task<SalaryRule?> GetSalaryRuleAsync(Guid salaryRuleId, Guid accountId);
        Task<SalaryRule> InsertSalaryRuleAsync(SalaryRule rule);
        Task UpdateSalaryRuleAsync(SalaryRule rule);
        Task SoftDeleteSalaryRuleAsync(SalaryRule rule);
        Task<bool> EmployeeCodeExistsAsync(Guid accountId, string employeeCode, Guid? excludeStaffId = null);
        Task<IReadOnlyList<string>> GetEmployeeCodesAsync(Guid accountId);
        Task<IReadOnlyList<string>> GetSpecialistsAsync(Guid accountId);
        Task AssignBranchEmployeeAsync(Guid userId, Guid branchId, string? photo);
        Task RemoveBranchEmployeesForUserAsync(Guid userId);
        Task<Staff?> GetByUserIdAsync(Guid userId, Guid accountId);
    }
}
