using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Interface for User business logic service.
    /// Defines contracts for user-related business operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user with validation and password hashing
        /// </summary>
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);

        /// <summary>
        /// Updates an existing user with validation
        /// </summary>
        Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto);

        /// <summary>
        /// Updates user password with hashing
        /// </summary>
        Task<bool> UpdateUserPasswordAsync(UpdateUserPasswordDto updatePasswordDto);

        /// <summary>
        /// Deletes a user (soft delete)
        /// </summary>
        Task<bool> DeleteUserAsync(Guid userId);

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        Task<UserDto?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Retrieves all users
        /// </summary>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves all users for a specific account
        /// </summary>
        Task<IEnumerable<UserDto>> GetUsersByAccountIdAsync(Guid accountId);

        /// <summary>
        /// Retrieves a user by email
        /// </summary>
        Task<UserDto?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Retrieves all users managed by a specific manager
        /// </summary>
        Task<IEnumerable<UserDto>> GetUsersByManagerIdAsync(Guid managerId);

        /// <summary>
        /// Validates if email is unique
        /// </summary>
        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeUserId = null);

        /// <summary>
        /// Validates if mobile is unique
        /// </summary>
        Task<bool> IsMobileUniqueAsync(string mobile, Guid? excludeUserId = null);
    }
}
