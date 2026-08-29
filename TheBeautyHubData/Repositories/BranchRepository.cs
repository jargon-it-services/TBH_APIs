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
    public class BranchRepository : IBranchRepository
    {
        private readonly BeautyHubDbContext _context;

        public BranchRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync(Guid accountId)
        {
            return await _context.Branches
                .AsNoTracking()
                .Where(b => !b.IsDeleted && b.AccountId == accountId)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Branch?> GetByIdAsync(Guid branchId)
        {
            return await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchId == branchId && !b.IsDeleted);
        }

        public async Task<Branch?> GetDetailsByIdAsync(Guid branchId)
        {
            return await _context.Branches
                .Include(b => b.BranchServices)
                    .ThenInclude(bs => bs.Service)
                .Include(b => b.BranchEmployees)
                .AsSplitQuery()
                .FirstOrDefaultAsync(b => b.BranchId == branchId && !b.IsDeleted);
        }

        public async Task<Branch> InsertAsync(Branch branch)
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<Branch> UpdateAsync(Branch branch)
        {
            branch.LastUpdated = DateTime.UtcNow;
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task ReplaceServicesAsync(Guid branchId, IEnumerable<Guid> serviceIds)
        {
            var existing = await _context.BranchServices
                .Where(bs => bs.BranchId == branchId)
                .ToListAsync();

            _context.BranchServices.RemoveRange(existing);

            var distinctIds = serviceIds.Distinct().ToList();
            foreach (var serviceId in distinctIds)
            {
                _context.BranchServices.Add(new BranchService
                {
                    BranchId = branchId,
                    ServiceId = serviceId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Services>> GetServicesByIdsAsync(IEnumerable<Guid> serviceIds)
        {
            var ids = serviceIds.Distinct().ToList();
            if (ids.Count == 0)
                return Array.Empty<Services>();

            return await _context.Services
                .Where(s => ids.Contains(s.ServiceId) && !s.IsDeleted)
                .ToListAsync();
        }
    }
}
