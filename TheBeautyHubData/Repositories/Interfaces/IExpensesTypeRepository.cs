using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for ExpensesType repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IExpensesTypeRepository
    {
        Task<ExpensesType> InsertExpensesTypeAsync(ExpensesType expensesType);
        Task<ExpensesType> UpdateExpensesTypeAsync(ExpensesType expensesType);
        Task<int> DeleteExpensesTypeAsync(Guid expensesTypeId);
        Task<ExpensesType?> GetExpensesTypeByIdAsync(Guid expensesTypeId);
        Task<IEnumerable<ExpensesType>> GetExpensesTypesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<ExpensesType>> GetAllExpensesTypesAsync();
    }
}
