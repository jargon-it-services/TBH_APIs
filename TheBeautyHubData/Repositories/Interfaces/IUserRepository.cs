using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for User repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Inserts a new user using stored procedure
        /// </summary>
        Task<User> InsertUserAsync(User user);

        /// <summary>
        /// Updates an existing user using stored procedure
        /// </summary>
        Task<User> UpdateUserAsync(User user);

        /// <summary>
        /// Updates user password hash using stored procedure
        /// </summary>
        Task<int> UpdateUserPasswordAsync(Guid userId, byte[] passwordHash);

        /// <summary>
        /// Soft deletes a user using stored procedure
        /// </summary>
        Task<int> DeleteUserAsync(Guid userId);

        /// <summary>
        /// Retrieves a user by ID using stored procedure
        /// </summary>
        Task<User?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Retrieves all non-deleted users using stored procedure
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves all users for a specific account using stored procedure
        /// </summary>
        Task<IEnumerable<User>> GetUsersByAccountIdAsync(Guid accountId);

        /// <summary>
        /// Retrieves a user by email using stored procedure
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Retrieves all users managed by a specific manager using stored procedure
        /// </summary>
        Task<IEnumerable<User>> GetUsersByManagerIdAsync(Guid managerId);
    }
}
