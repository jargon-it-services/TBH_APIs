using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IExpensesTypeRepository
    {
        Task<IReadOnlyList<ExpensesType>> GetByAccountIdAsync(Guid accountId);
        Task<ExpensesType?> GetByIdAsync(Guid expensesTypeId, Guid accountId);
        Task<ExpensesType?> GetDetailsByIdAsync(Guid expensesTypeId, Guid accountId);
        Task<ExpensesType> InsertAsync(ExpensesType expensesType);
        Task UpdateAsync(ExpensesType expensesType);
        Task SoftDeleteAsync(ExpensesType expensesType);
        Task ReplaceBranchesAsync(Guid expensesTypeId, IEnumerable<Guid> branchIds);
        Task<IReadOnlyList<Branch>> GetBranchesByIdsAsync(Guid accountId, IEnumerable<Guid> branchIds);
        Task RemoveBranchLinksAsync(Guid expensesTypeId);
    }
}
