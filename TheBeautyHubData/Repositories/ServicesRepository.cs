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
    public class ServicesRepository : IServicesRepository
    {
        private readonly BeautyHubDbContext _context;

        public ServicesRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Services>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Services
                .AsNoTracking()
                .Where(s => !s.IsDeleted && s.AccountId == accountId)
                .OrderBy(s => s.ServiceName)
                .ToListAsync();
        }

        public async Task<Services?> GetByIdAsync(Guid serviceId, Guid accountId)
        {
            return await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.AccountId == accountId && !s.IsDeleted);
        }

        public async Task<Services?> GetDetailsByIdAsync(Guid serviceId, Guid accountId)
        {
            return await _context.Services
                .Include(s => s.BranchServices)
                    .ThenInclude(bs => bs.Branch)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.AccountId == accountId && !s.IsDeleted);
        }

        public async Task<Services> InsertAsync(Services service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task UpdateAsync(Services service)
        {
            service.LastUpdated = DateTime.UtcNow;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Services service)
        {
            service.IsDeleted = true;
            service.LastUpdated = DateTime.UtcNow;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceBranchesAsync(Guid serviceId, IEnumerable<Guid> branchIds)
        {
            var existing = await _context.BranchServices
                .Where(bs => bs.ServiceId == serviceId)
                .ToListAsync();

            _context.BranchServices.RemoveRange(existing);

            foreach (var branchId in branchIds.Distinct())
            {
                _context.BranchServices.Add(new BranchService
                {
                    BranchId = branchId,
                    ServiceId = serviceId
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

        public async Task RemoveBranchLinksAsync(Guid serviceId)
        {
            var existing = await _context.BranchServices
                .Where(bs => bs.ServiceId == serviceId)
                .ToListAsync();

            if (existing.Count == 0)
                return;

            _context.BranchServices.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }
    }
}
