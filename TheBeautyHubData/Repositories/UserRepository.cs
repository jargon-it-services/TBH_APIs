using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    /// <summary>
    /// Repository implementation for User entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly BeautyHubDbContext _context;

        public UserRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inserts a new user using the usp_Insert_User stored procedure
        /// </summary>
        public async Task<User> InsertUserAsync(User user)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountId", user.AccountId),
                new SqlParameter("@UserRole", user.UserRole),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@UserEmail", (object?)user.UserEmail ?? DBNull.Value),
                new SqlParameter("@UserMobile", (object?)user.UserMobile ?? DBNull.Value),
                new SqlParameter("@UserPasswordHash", user.UserPasswordHash),
                new SqlParameter("@EmailVerified", user.EmailVerified),
                new SqlParameter("@MobileVerified", user.MobileVerified),
                new SqlParameter("@WorkerPaymentType", (object?)user.WorkerPaymentType ?? DBNull.Value),
                new SqlParameter("@ManagerId", (object?)user.ManagerId ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)user.CreatedBy ?? DBNull.Value),
                new SqlParameter("@Status", user.Status)
            };

            var result = await _context.Users
                .FromSqlRaw("EXEC usp_Insert_User @AccountId, @UserRole, @UserName, @UserEmail, @UserMobile, @UserPasswordHash, @EmailVerified, @MobileVerified, @WorkerPaymentType, @ManagerId, @CreatedBy, @Status", 
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? user;
        }

        /// <summary>
        /// Updates an existing user using the usp_Update_User stored procedure
        /// </summary>
        public async Task<User> UpdateUserAsync(User user)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserRole", user.UserRole),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@UserEmail", (object?)user.UserEmail ?? DBNull.Value),
                new SqlParameter("@UserMobile", (object?)user.UserMobile ?? DBNull.Value),
                new SqlParameter("@EmailVerified", user.EmailVerified),
                new SqlParameter("@MobileVerified", user.MobileVerified),
                new SqlParameter("@WorkerPaymentType", (object?)user.WorkerPaymentType ?? DBNull.Value),
                new SqlParameter("@ManagerId", (object?)user.ManagerId ?? DBNull.Value),
                new SqlParameter("@Status", user.Status)
            };

            var result = await _context.Users
                .FromSqlRaw("EXEC usp_Update_User @UserId, @UserRole, @UserName, @UserEmail, @UserMobile, @EmailVerified, @MobileVerified, @WorkerPaymentType, @ManagerId, @Status", 
                    parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? user;
        }

        /// <summary>
        /// Updates user password hash using the usp_Update_UserPassword stored procedure
        /// </summary>
        public async Task<int> UpdateUserPasswordAsync(Guid userId, byte[] passwordHash)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@UserPasswordHash", passwordHash)
            };
            
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Update_UserPassword @UserId, @UserPasswordHash", parameters);
        }

        /// <summary>
        /// Soft deletes a user using the usp_Delete_User stored procedure
        /// </summary>
        public async Task<int> DeleteUserAsync(Guid userId)
        {
            var parameter = new SqlParameter("@UserId", userId);
            
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Delete_User @UserId", parameter);
        }

        /// <summary>
        /// Retrieves a user by ID using the usp_Get_UserById stored procedure
        /// </summary>
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var parameter = new SqlParameter("@UserId", userId);
            
            var result = await _context.Users
                .FromSqlRaw("EXEC usp_Get_UserById @UserId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all non-deleted users using the usp_Get_AllUsers stored procedure
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .FromSqlRaw("EXEC usp_Get_AllUsers")
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all users for a specific account using the usp_Get_UsersByAccountId stored procedure
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            
            return await _context.Users
                .FromSqlRaw("EXEC usp_Get_UsersByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by email using the usp_Get_UserByEmail stored procedure
        /// </summary>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var parameter = new SqlParameter("@UserEmail", email);
            
            var result = await _context.Users
                .FromSqlRaw("EXEC usp_Get_UserByEmail @UserEmail", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all users managed by a specific manager using the usp_Get_UsersByManagerId stored procedure
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByManagerIdAsync(Guid managerId)
        {
            var parameter = new SqlParameter("@ManagerId", managerId);
            
            return await _context.Users
                .FromSqlRaw("EXEC usp_Get_UsersByManagerId @ManagerId", parameter)
                .ToListAsync();
        }
    }
}
