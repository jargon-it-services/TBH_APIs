using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Interface for Account business logic service.
    /// Defines contracts for account-related business operations.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Creates a new account with validation
        /// </summary>
        Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto);

        /// <summary>
        /// Updates an existing account with validation
        /// </summary>
        Task<AccountDto> UpdateAccountAsync(UpdateAccountDto updateAccountDto);

        /// <summary>
        /// Deletes an account (soft delete)
        /// </summary>
        Task<bool> DeleteAccountAsync(Guid accountId);

        /// <summary>
        /// Retrieves an account by ID
        /// </summary>
        Task<AccountDto?> GetAccountByIdAsync(Guid accountId);

        /// <summary>
        /// Retrieves all accounts
        /// </summary>
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();

        /// <summary>
        /// Retrieves an account by its unique code
        /// </summary>
        Task<AccountDto?> GetAccountByCodeAsync(string accountCode);

        /// <summary>
        /// Validates if account code is unique
        /// </summary>
        Task<bool> IsAccountCodeUniqueAsync(string accountCode, Guid? excludeAccountId = null);
    }
}
