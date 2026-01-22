using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Service interface for ExpensesType operations.
    /// Defines business logic layer contract for expenses type management.
    /// </summary>
    public interface IExpensesTypeService
    {
        Task<ExpensesTypeDto> CreateExpensesTypeAsync(CreateExpensesTypeDto createExpensesTypeDto);
        Task<ExpensesTypeDto> UpdateExpensesTypeAsync(UpdateExpensesTypeDto updateExpensesTypeDto);
        Task<bool> DeleteExpensesTypeAsync(Guid expensesTypeId);
        Task<ExpensesTypeDto?> GetExpensesTypeByIdAsync(Guid expensesTypeId);
        Task<IEnumerable<ExpensesTypeDto>> GetExpensesTypesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<ExpensesTypeDto>> GetAllExpensesTypesAsync();
    }
}
