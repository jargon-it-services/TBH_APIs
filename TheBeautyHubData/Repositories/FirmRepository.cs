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
    public class FirmRepository : IFirmRepository
    {
        private readonly BeautyHubDbContext _context;

        public FirmRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Firm> InsertFirmAsync(Firm firm)
        {
            _context.Firms.Add(firm);
            await _context.SaveChangesAsync();
            return firm;
        }

        public async Task<Firm> UpdateFirmAsync(Firm firm)
        {
            _context.Firms.Update(firm);
            await _context.SaveChangesAsync();
            return firm;
        }

        public async Task<int> DeleteFirmAsync(Guid firmId)
        {
            var firm = await _context.Firms.FindAsync(firmId);
            if (firm == null) return 0;
            
            firm.IsDeleted = true;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<Firm?> GetFirmByIdAsync(Guid firmId)
        {
            return await _context.Firms
                .Where(f => f.FirmId == firmId && !f.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Firm>> GetAllFirmsAsync()
        {
            return await _context.Firms
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Firm>> GetFirmsByAccountIdAsync(Guid accountId)
        {
            return await _context.Firms
                .Where(f => f.AccountId == accountId && !f.IsDeleted)
                .ToListAsync();
        }
    }
}
