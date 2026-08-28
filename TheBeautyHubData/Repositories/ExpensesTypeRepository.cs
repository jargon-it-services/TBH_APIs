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
    public class ExpensesTypeRepository : IExpensesTypeRepository
    {
        private readonly BeautyHubDbContext _context;

        public ExpensesTypeRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ExpensesType>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.ExpensesTypes
                .AsNoTracking()
                .Include(e => e.ExpenseBranches)
                    .ThenInclude(eb => eb.Branch)
                .Where(e => !e.IsDeleted && e.AccountId == accountId)
                .OrderBy(e => e.ExpensesTypeName)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<ExpensesType?> GetByIdAsync(Guid expensesTypeId, Guid accountId)
        {
            return await _context.ExpensesTypes
                .FirstOrDefaultAsync(e => e.ExpensesTypeId == expensesTypeId && e.AccountId == accountId && !e.IsDeleted);
        }

        public async Task<ExpensesType?> GetDetailsByIdAsync(Guid expensesTypeId, Guid accountId)
        {
            return await _context.ExpensesTypes
                .Include(e => e.ExpenseBranches)
                    .ThenInclude(eb => eb.Branch)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.ExpensesTypeId == expensesTypeId && e.AccountId == accountId && !e.IsDeleted);
        }

        public async Task<ExpensesType> InsertAsync(ExpensesType expensesType)
        {
            _context.ExpensesTypes.Add(expensesType);
            await _context.SaveChangesAsync();
            return expensesType;
        }

        public async Task UpdateAsync(ExpensesType expensesType)
        {
            expensesType.LastUpdated = DateTime.UtcNow;
            _context.ExpensesTypes.Update(expensesType);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(ExpensesType expensesType)
        {
            expensesType.IsDeleted = true;
            expensesType.LastUpdated = DateTime.UtcNow;
            _context.ExpensesTypes.Update(expensesType);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceBranchesAsync(Guid expensesTypeId, IEnumerable<Guid> branchIds)
        {
            var existing = await _context.ExpensesTypeBranches
                .Where(eb => eb.ExpensesTypeId == expensesTypeId)
                .ToListAsync();

            _context.ExpensesTypeBranches.RemoveRange(existing);

            foreach (var branchId in branchIds.Distinct())
            {
                _context.ExpensesTypeBranches.Add(new ExpensesTypeBranch
                {
                    ExpensesTypeId = expensesTypeId,
                    BranchId = branchId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Branch>> GetBranchesByIdsAsync(Guid accountId, IEnumerable<Guid> branchIds)
        {
            var ids = branchIds.Distinct().ToList();
            if (ids.Count == 0)
                return Array.Empty<Branch>();

            return await _context.Branches
                .Where(b => ids.Contains(b.BranchId) && b.AccountId == accountId && !b.IsDeleted)
                .ToListAsync();
        }

        public async Task RemoveBranchLinksAsync(Guid expensesTypeId)
        {
            var existing = await _context.ExpensesTypeBranches
                .Where(eb => eb.ExpensesTypeId == expensesTypeId)
                .ToListAsync();

            if (existing.Count == 0)
                return;

            _context.ExpensesTypeBranches.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }
    }
}
