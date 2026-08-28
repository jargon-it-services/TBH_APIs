using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private static readonly string[] DefaultSalaryRuleNames =
        {
            "Fixed Pay",
            "Fixed + Target Bonus",
            "Incentive"
        };

        private readonly BeautyHubDbContext _context;

        public StaffRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Staff>> GetAllAsync(Guid accountId)
        {
            return await _context.StaffMembers
                .AsNoTracking()
                .Include(s => s.Branch)
                .Where(s => s.AccountId == accountId && !s.IsDeleted)
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }

        public async Task<Staff?> GetByIdAsync(Guid staffId, Guid accountId)
        {
            return await _context.StaffMembers
                .Include(s => s.Branch)
                .Include(s => s.SalaryRule)
                .FirstOrDefaultAsync(s => s.StaffId == staffId && s.AccountId == accountId && !s.IsDeleted);
        }

        public async Task<Staff> InsertAsync(Staff staff)
        {
            _context.StaffMembers.Add(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<Staff> UpdateAsync(Staff staff)
        {
            staff.LastUpdated = DateTime.UtcNow;
            _context.StaffMembers.Update(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task SoftDeleteAsync(Staff staff)
        {
            staff.IsDeleted = true;
            staff.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<SalaryRule>> GetSalaryRulesAsync(Guid accountId)
        {
            return await _context.SalaryRules
                .AsNoTracking()
                .Where(r => r.AccountId == accountId && !r.IsDeleted)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task EnsureDefaultSalaryRulesAsync(Guid accountId)
        {
            var exists = await _context.SalaryRules.AnyAsync(r => r.AccountId == accountId && !r.IsDeleted);
            if (exists)
                return;

            foreach (var name in DefaultSalaryRuleNames)
            {
                _context.SalaryRules.Add(new SalaryRule
                {
                    AccountId = accountId,
                    Name = name,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<SalaryRule?> GetSalaryRuleAsync(Guid salaryRuleId, Guid accountId)
        {
            return await _context.SalaryRules
                .FirstOrDefaultAsync(r => r.SalaryRuleId == salaryRuleId && r.AccountId == accountId && !r.IsDeleted);
        }

        public async Task<bool> EmployeeCodeExistsAsync(Guid accountId, string employeeCode, Guid? excludeStaffId = null)
        {
            var query = _context.StaffMembers.Where(s =>
                s.AccountId == accountId &&
                !s.IsDeleted &&
                s.EmployeeCode == employeeCode);

            if (excludeStaffId.HasValue)
                query = query.Where(s => s.StaffId != excludeStaffId.Value);

            return await query.AnyAsync();
        }

        public async Task<IReadOnlyList<string>> GetEmployeeCodesAsync(Guid accountId)
        {
            return await _context.StaffMembers
                .AsNoTracking()
                .Where(s => s.AccountId == accountId && !s.IsDeleted && s.EmployeeCode != null)
                .Select(s => s.EmployeeCode!)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetSpecialistsAsync(Guid accountId)
        {
            return await _context.StaffMembers
                .AsNoTracking()
                .Where(s => s.AccountId == accountId && !s.IsDeleted && s.Specialist != "")
                .Select(s => s.Specialist)
                .Distinct()
                .ToListAsync();
        }

        public async Task AssignBranchEmployeeAsync(Guid userId, Guid branchId, string? photo)
        {
            await RemoveBranchEmployeesForUserAsync(userId);
            _context.BranchEmployees.Add(new BranchEmployee
            {
                UserId = userId,
                BranchId = branchId,
                Photo = photo
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBranchEmployeesForUserAsync(Guid userId)
        {
            var existing = await _context.BranchEmployees.Where(be => be.UserId == userId).ToListAsync();
            if (existing.Count == 0)
                return;

            _context.BranchEmployees.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }
    }
}
