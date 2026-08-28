using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    /// <summary>
    /// Business logic service for User operations.
    /// Implements validation, business rules, password hashing, and orchestrates repository calls.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public UserService(IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Creates a new user with business validation and password hashing
        /// </summary>
        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Business validations
            if (createUserDto.AccountId == Guid.Empty)
                throw new ArgumentException("Account ID is required.");

            // Verify account exists
            var account = await _accountRepository.GetAccountByIdAsync(createUserDto.AccountId);
            if (account == null)
                throw new InvalidOperationException("Account not found.");

            if (string.IsNullOrWhiteSpace(createUserDto.UserName))
                throw new ArgumentException("User name is required.");

            if (!IsValidUserRole(createUserDto.UserRole))
                throw new ArgumentException("User role must be 'Admin', 'Manager', or 'Employee'.");

            // Passwords and login are owned by AuthCenter; TBH stores no login credentials.

            // Validate email uniqueness if provided
            if (!string.IsNullOrWhiteSpace(createUserDto.UserEmail))
            {
                if (!await IsEmailUniqueAsync(createUserDto.UserEmail))
                    throw new InvalidOperationException("Email already exists.");
            }

            // Validate mobile uniqueness if provided
            if (!string.IsNullOrWhiteSpace(createUserDto.UserMobile))
            {
                if (!await IsMobileUniqueAsync(createUserDto.UserMobile))
                    throw new InvalidOperationException("Mobile number already exists.");
            }

            // Validate worker payment type if provided
            if (!string.IsNullOrWhiteSpace(createUserDto.WorkerPaymentType) && 
                !IsValidWorkerPaymentType(createUserDto.WorkerPaymentType))
            {
                throw new ArgumentException("Worker payment type must be 'Fix Pay', 'FP + Incentive', or 'Incentive'.");
            }

            // Validate manager exists if provided
            if (createUserDto.ManagerId.HasValue && createUserDto.ManagerId.Value != Guid.Empty)
            {
                var manager = await _userRepository.GetUserByIdAsync(createUserDto.ManagerId.Value);
                if (manager == null)
                    throw new InvalidOperationException("Manager not found.");
            }

            // AuthCenter owns credentials; persist an empty hash so the required column is satisfied.
            var passwordHash = Array.Empty<byte>();

            // Map DTO to Entity
            var user = new User
            {
                AccountId = createUserDto.AccountId,
                UserRole = createUserDto.UserRole,
                UserName = createUserDto.UserName,
                UserEmail = createUserDto.UserEmail,
                UserMobile = createUserDto.UserMobile,
                UserPasswordHash = passwordHash,
                EmailVerified = createUserDto.EmailVerified,
                MobileVerified = createUserDto.MobileVerified,
                WorkerPaymentType = createUserDto.WorkerPaymentType,
                ManagerId = createUserDto.ManagerId,
                CreatedBy = createUserDto.CreatedBy,
                Status = createUserDto.Status
            };

            // Save to database
            var createdUser = await _userRepository.InsertUserAsync(user);

            // Map Entity to DTO
            return MapToDto(createdUser);
        }

        /// <summary>
        /// Updates an existing user with business validation
        /// </summary>
        public async Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            // Business validations
            if (updateUserDto.UserId == Guid.Empty)
                throw new ArgumentException("User ID is required.");

            // Check if user exists
            var existingUser = await _userRepository.GetUserByIdAsync(updateUserDto.UserId);
            if (existingUser == null)
                throw new InvalidOperationException("User not found.");

            if (string.IsNullOrWhiteSpace(updateUserDto.UserName))
                throw new ArgumentException("User name is required.");

            if (!IsValidUserRole(updateUserDto.UserRole))
                throw new ArgumentException("User role must be 'Admin', 'Manager', or 'Employee'.");

            // Validate email uniqueness if provided (excluding current user)
            if (!string.IsNullOrWhiteSpace(updateUserDto.UserEmail))
            {
                if (!await IsEmailUniqueAsync(updateUserDto.UserEmail, updateUserDto.UserId))
                    throw new InvalidOperationException("Email already exists.");
            }

            // Validate mobile uniqueness if provided (excluding current user)
            if (!string.IsNullOrWhiteSpace(updateUserDto.UserMobile))
            {
                if (!await IsMobileUniqueAsync(updateUserDto.UserMobile, updateUserDto.UserId))
                    throw new InvalidOperationException("Mobile number already exists.");
            }

            // Validate worker payment type if provided
            if (!string.IsNullOrWhiteSpace(updateUserDto.WorkerPaymentType) && 
                !IsValidWorkerPaymentType(updateUserDto.WorkerPaymentType))
            {
                throw new ArgumentException("Worker payment type must be 'Fix Pay', 'FP + Incentive', or 'Incentive'.");
            }

            // Validate manager exists if provided
            if (updateUserDto.ManagerId.HasValue && updateUserDto.ManagerId.Value != Guid.Empty)
            {
                // Prevent self-referencing
                if (updateUserDto.ManagerId.Value == updateUserDto.UserId)
                    throw new InvalidOperationException("User cannot be their own manager.");

                var manager = await _userRepository.GetUserByIdAsync(updateUserDto.ManagerId.Value);
                if (manager == null)
                    throw new InvalidOperationException("Manager not found.");
            }

            // Map DTO to Entity
            var user = new User
            {
                UserId = updateUserDto.UserId,
                UserRole = updateUserDto.UserRole,
                UserName = updateUserDto.UserName,
                UserEmail = updateUserDto.UserEmail,
                UserMobile = updateUserDto.UserMobile,
                EmailVerified = updateUserDto.EmailVerified,
                MobileVerified = updateUserDto.MobileVerified,
                WorkerPaymentType = updateUserDto.WorkerPaymentType,
                ManagerId = updateUserDto.ManagerId,
                Status = updateUserDto.Status
            };

            // Update in database
            var updatedUser = await _userRepository.UpdateUserAsync(user);

            // Map Entity to DTO
            return MapToDto(updatedUser);
        }

        /// <summary>
        /// Updates user password with hashing
        /// </summary>
        public async Task<bool> UpdateUserPasswordAsync(UpdateUserPasswordDto updatePasswordDto)
        {
            if (updatePasswordDto.UserId == Guid.Empty)
                throw new ArgumentException("User ID is required.");

            if (string.IsNullOrWhiteSpace(updatePasswordDto.NewPassword))
                throw new ArgumentException("New password is required.");

            if (updatePasswordDto.NewPassword.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            // Check if user exists
            var existingUser = await _userRepository.GetUserByIdAsync(updatePasswordDto.UserId);
            if (existingUser == null)
                throw new InvalidOperationException("User not found.");

            // Hash new password
            var passwordHash = HashPassword(updatePasswordDto.NewPassword);

            // Update password in database
            var result = await _userRepository.UpdateUserPasswordAsync(updatePasswordDto.UserId, passwordHash);
            return result > 0;
        }

        /// <summary>
        /// Deletes a user (soft delete)
        /// </summary>
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required.");

            var result = await _userRepository.DeleteUserAsync(userId);
            return result > 0;
        }

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required.");

            var user = await _userRepository.GetUserByIdAsync(userId);
            return user != null ? MapToDto(user) : null;
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(MapToDto);
        }

        /// <summary>
        /// Retrieves all users for a specific account
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetUsersByAccountIdAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Account ID is required.");

            var users = await _userRepository.GetUsersByAccountIdAsync(accountId);
            return users.Select(MapToDto);
        }

        /// <summary>
        /// Retrieves a user by email
        /// </summary>
        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        /// <summary>
        /// Retrieves all users managed by a specific manager
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetUsersByManagerIdAsync(Guid managerId)
        {
            if (managerId == Guid.Empty)
                throw new ArgumentException("Manager ID is required.");

            var users = await _userRepository.GetUsersByManagerIdAsync(managerId);
            return users.Select(MapToDto);
        }

        /// <summary>
        /// Validates if email is unique
        /// </summary>
        public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            
            if (existingUser == null)
                return true;

            if (excludeUserId.HasValue && existingUser.UserId == excludeUserId.Value)
                return true;

            return false;
        }

        /// <summary>
        /// Validates if mobile is unique
        /// </summary>
        public Task<bool> IsMobileUniqueAsync(string mobile, Guid? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return Task.FromResult(true);

            // Note: This requires adding a GetUserByMobile method to repository
            // For now, we'll return true as the database constraint will catch duplicates
            return Task.FromResult(true);
        }

        // Helper methods
        private bool IsValidUserRole(string userRole)
        {
            return userRole == "Admin" || userRole == "Manager" || userRole == "Employee";
        }

        private bool IsValidWorkerPaymentType(string paymentType)
        {
            return paymentType == "Fix Pay" || paymentType == "FP + Incentive" || paymentType == "Incentive";
        }

        /// <summary>
        /// Hashes a password using SHA256 (for demonstration - use BCrypt or Argon2 in production)
        /// </summary>
        private byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                AccountId = user.AccountId,
                UserRole = user.UserRole,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserMobile = user.UserMobile,
                EmailVerified = user.EmailVerified,
                MobileVerified = user.MobileVerified,
                WorkerPaymentType = user.WorkerPaymentType,
                ManagerId = user.ManagerId,
                Status = user.Status,
                CreatedAt = user.CreatedAt,
                LastUpdated = user.LastUpdated
            };
        }
    }
}
