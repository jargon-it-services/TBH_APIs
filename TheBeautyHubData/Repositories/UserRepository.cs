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
    public class UserRepository : IUserRepository
    {
        private readonly BeautyHubDbContext _context;

        public UserRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<User> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            user.LastUpdated = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<int> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0;
            
            user.IsDeleted = true;
            user.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.UserEmail == email && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByMobileAsync(string mobile)
        {
            return await _context.Users
                .Where(u => u.UserMobile == mobile && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByAccountIdAsync(Guid accountId)
        {
            return await _context.Users
                .Where(u => u.AccountId == accountId && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> UpdateUserPasswordAsync(Guid userId, byte[] passwordHash)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0;

            user.UserPasswordHash = passwordHash;
            user.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<IEnumerable<User>> GetUsersByManagerIdAsync(Guid managerId)
        {
            return await _context.Users
                .Where(u => u.ManagerId == managerId && !u.IsDeleted)
                .ToListAsync();
        }
    }
}
