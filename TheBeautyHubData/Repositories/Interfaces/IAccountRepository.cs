using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Account repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Inserts a new account using stored procedure
        /// </summary>
        Task<Account> InsertAccountAsync(Account account);

        /// <summary>
        /// Updates an existing account using stored procedure
        /// </summary>
        Task<Account> UpdateAccountAsync(Account account);

        /// <summary>
        /// Soft deletes an account using stored procedure
        /// </summary>
        Task<int> DeleteAccountAsync(Guid accountId);

        /// <summary>
        /// Retrieves an account by ID using stored procedure
        /// </summary>
        Task<Account?> GetAccountByIdAsync(Guid accountId);

        /// <summary>
        /// Retrieves all non-deleted accounts using stored procedure
        /// </summary>
        Task<IEnumerable<Account>> GetAllAccountsAsync();

        /// <summary>
        /// Retrieves an account by its unique code using stored procedure
        /// </summary>
        Task<Account?> GetAccountByCodeAsync(string accountCode);
    }
}
