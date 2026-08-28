using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IExpensesTypeService
    {
        Task<IReadOnlyList<ExpenseListItemDto>> GetListAsync(Guid accountId);
        Task<ExpenseDetailDto?> GetDetailsAsync(Guid expenseId, Guid accountId);
        Task CreateAsync(SaveExpenseDto dto);
        Task UpdateAsync(Guid expenseId, SaveExpenseDto dto);
        Task DeleteAsync(Guid expenseId, Guid accountId);
    }
}
