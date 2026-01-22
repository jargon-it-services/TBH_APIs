using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    /// <summary>
    /// Business logic service for Account operations.
    /// Implements validation, business rules, and orchestrates repository calls.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Creates a new account with business validation
        /// </summary>
        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            // Business validations
            if (string.IsNullOrWhiteSpace(createAccountDto.AccountCode))
                throw new ArgumentException("Account code is required.");

            if (createAccountDto.AccountCode.Length < 6)
                throw new ArgumentException("Account code must be at least 6 characters.");

            if (string.IsNullOrWhiteSpace(createAccountDto.AccountName))
                throw new ArgumentException("Account name is required.");

            if (!IsValidAccountType(createAccountDto.AccountType))
                throw new ArgumentException("Account type must be 'FirmOwner' or 'Customer'.");

            if (!IsValidMode(createAccountDto.Mode))
                throw new ArgumentException("Mode must be 'subscription' or 'one_time'.");

            // Check if account code is unique
            if (!await IsAccountCodeUniqueAsync(createAccountDto.AccountCode))
                throw new InvalidOperationException("Account code already exists.");

            // Validate trial period logic
            if (createAccountDto.IsUnderTrial)
            {
                if (!createAccountDto.TrialStartedOn.HasValue)
                    throw new ArgumentException("Trial start date is required when account is under trial.");

                if (!createAccountDto.TrialDuration.HasValue || createAccountDto.TrialDuration <= 0)
                    throw new ArgumentException("Trial duration must be greater than 0 when account is under trial.");

                // Calculate trial expiration if not provided
                if (!createAccountDto.TrialExpiredOn.HasValue)
                {
                    createAccountDto.TrialExpiredOn = createAccountDto.TrialStartedOn.Value
                        .AddDays(createAccountDto.TrialDuration.Value);
                }
            }

            // Map DTO to Entity
            var account = new Account
            {
                AccountCode = createAccountDto.AccountCode,
                AccountName = createAccountDto.AccountName,
                AccountType = createAccountDto.AccountType,
                Mode = createAccountDto.Mode,
                IsUnderTrial = createAccountDto.IsUnderTrial,
                TrialStartedOn = createAccountDto.TrialStartedOn,
                TrialDuration = createAccountDto.TrialDuration,
                TrialExpiredOn = createAccountDto.TrialExpiredOn,
                CreatedBy = createAccountDto.CreatedBy
            };

            // Save to database
            var createdAccount = await _accountRepository.InsertAccountAsync(account);

            // Map Entity to DTO
            return MapToDto(createdAccount);
        }

        /// <summary>
        /// Updates an existing account with business validation
        /// </summary>
        public async Task<AccountDto> UpdateAccountAsync(UpdateAccountDto updateAccountDto)
        {
            // Business validations
            if (updateAccountDto.AccountId == Guid.Empty)
                throw new ArgumentException("Account ID is required.");

            // Check if account exists
            var existingAccount = await _accountRepository.GetAccountByIdAsync(updateAccountDto.AccountId);
            if (existingAccount == null)
                throw new InvalidOperationException("Account not found.");

            if (string.IsNullOrWhiteSpace(updateAccountDto.AccountCode))
                throw new ArgumentException("Account code is required.");

            if (updateAccountDto.AccountCode.Length < 6)
                throw new ArgumentException("Account code must be at least 6 characters.");

            if (string.IsNullOrWhiteSpace(updateAccountDto.AccountName))
                throw new ArgumentException("Account name is required.");

            if (!IsValidAccountType(updateAccountDto.AccountType))
                throw new ArgumentException("Account type must be 'FirmOwner' or 'Customer'.");

            if (!IsValidMode(updateAccountDto.Mode))
                throw new ArgumentException("Mode must be 'subscription' or 'one_time'.");

            // Check if account code is unique (excluding current account)
            if (!await IsAccountCodeUniqueAsync(updateAccountDto.AccountCode, updateAccountDto.AccountId))
                throw new InvalidOperationException("Account code already exists.");

            // Map DTO to Entity
            var account = new Account
            {
                AccountId = updateAccountDto.AccountId,
                AccountCode = updateAccountDto.AccountCode,
                AccountName = updateAccountDto.AccountName,
                AccountType = updateAccountDto.AccountType,
                Mode = updateAccountDto.Mode,
                IsUnderTrial = updateAccountDto.IsUnderTrial,
                TrialStartedOn = updateAccountDto.TrialStartedOn,
                TrialDuration = updateAccountDto.TrialDuration,
                TrialExpiredOn = updateAccountDto.TrialExpiredOn
            };

            // Update in database
            var updatedAccount = await _accountRepository.UpdateAccountAsync(account);

            // Map Entity to DTO
            return MapToDto(updatedAccount);
        }

        /// <summary>
        /// Deletes an account (soft delete)
        /// </summary>
        public async Task<bool> DeleteAccountAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Account ID is required.");

            var result = await _accountRepository.DeleteAccountAsync(accountId);
            return result > 0;
        }

        /// <summary>
        /// Retrieves an account by ID
        /// </summary>
        public async Task<AccountDto?> GetAccountByIdAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Account ID is required.");

            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            return account != null ? MapToDto(account) : null;
        }

        /// <summary>
        /// Retrieves all accounts
        /// </summary>
        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();
            return accounts.Select(MapToDto);
        }

        /// <summary>
        /// Retrieves an account by its unique code
        /// </summary>
        public async Task<AccountDto?> GetAccountByCodeAsync(string accountCode)
        {
            if (string.IsNullOrWhiteSpace(accountCode))
                throw new ArgumentException("Account code is required.");

            var account = await _accountRepository.GetAccountByCodeAsync(accountCode);
            return account != null ? MapToDto(account) : null;
        }

        /// <summary>
        /// Validates if account code is unique
        /// </summary>
        public async Task<bool> IsAccountCodeUniqueAsync(string accountCode, Guid? excludeAccountId = null)
        {
            var existingAccount = await _accountRepository.GetAccountByCodeAsync(accountCode);
            
            if (existingAccount == null)
                return true;

            if (excludeAccountId.HasValue && existingAccount.AccountId == excludeAccountId.Value)
                return true;

            return false;
        }

        // Helper methods
        private bool IsValidAccountType(string accountType)
        {
            return accountType == "FirmOwner" || accountType == "Customer";
        }

        private bool IsValidMode(string mode)
        {
            return mode == "subscription" || mode == "one_time";
        }

        private AccountDto MapToDto(Account account)
        {
            return new AccountDto
            {
                AccountId = account.AccountId,
                AccountCode = account.AccountCode,
                AccountName = account.AccountName,
                AccountType = account.AccountType,
                Mode = account.Mode,
                IsUnderTrial = account.IsUnderTrial,
                TrialStartedOn = account.TrialStartedOn,
                TrialDuration = account.TrialDuration,
                TrialExpiredOn = account.TrialExpiredOn,
                CreatedAt = account.CreatedAt,
                LastUpdated = account.LastUpdated
            };
        }
    }
}
