using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class ExpensesTypeService : IExpensesTypeService
    {
        private readonly IExpensesTypeRepository _expensesTypeRepository;

        public ExpensesTypeService(IExpensesTypeRepository expensesTypeRepository)
        {
            _expensesTypeRepository = expensesTypeRepository;
        }

        public async Task<IReadOnlyList<ExpenseListItemDto>> GetListAsync(Guid accountId)
        {
            var expenses = await _expensesTypeRepository.GetByAccountIdAsync(accountId);
            return expenses.Select(MapListItem).ToList();
        }

        public async Task<ExpenseDetailDto?> GetDetailsAsync(Guid expenseId, Guid accountId)
        {
            var expense = await _expensesTypeRepository.GetDetailsByIdAsync(expenseId, accountId);
            return expense == null ? null : MapDetail(expense);
        }

        public async Task CreateAsync(SaveExpenseDto dto)
        {
            ValidateWrite(dto);
            var branchIds = await ResolveBranchIdsAsync(dto);

            var expense = new ExpensesType
            {
                AccountId = dto.AccountId,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            ApplyFields(expense, dto);

            var inserted = await _expensesTypeRepository.InsertAsync(expense);
            await _expensesTypeRepository.ReplaceBranchesAsync(inserted.ExpensesTypeId, branchIds);
        }

        public async Task UpdateAsync(Guid expenseId, SaveExpenseDto dto)
        {
            ValidateWrite(dto);

            var existing = await _expensesTypeRepository.GetByIdAsync(expenseId, dto.AccountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.ExpenseNotFound);

            var branchIds = await ResolveBranchIdsAsync(dto);
            ApplyFields(existing, dto);
            await _expensesTypeRepository.UpdateAsync(existing);
            await _expensesTypeRepository.ReplaceBranchesAsync(existing.ExpensesTypeId, branchIds);
        }

        public async Task DeleteAsync(Guid expenseId, Guid accountId)
        {
            var existing = await _expensesTypeRepository.GetByIdAsync(expenseId, accountId);
            if (existing == null)
                throw new KeyNotFoundException(ApiMessages.ExpenseNotFound);

            await _expensesTypeRepository.RemoveBranchLinksAsync(expenseId);
            await _expensesTypeRepository.SoftDeleteAsync(existing);
        }

        private async Task<List<Guid>> ResolveBranchIdsAsync(SaveExpenseDto dto)
        {
            if (dto.AllBranches)
                return new List<Guid>();

            var requested = (dto.Branches ?? new List<Guid>()).Distinct().ToList();
            if (requested.Count == 0)
                throw new ArgumentException(ApiMessages.BranchesRequiredWhenNotAll);

            var found = await _expensesTypeRepository.GetBranchesByIdsAsync(dto.AccountId, requested);
            if (found.Count != requested.Count)
                throw new ArgumentException(ApiMessages.InvalidBranchIds);

            return requested;
        }

        private static void ApplyFields(ExpensesType expense, SaveExpenseDto dto)
        {
            expense.ExpensesTypeName = dto.Name.Trim();
            expense.Description = dto.Description.Trim();
            expense.AllBranches = dto.AllBranches;
            expense.Status = dto.Status.Trim();
        }

        private static void ValidateWrite(SaveExpenseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(ApiMessages.ExpenseNameRequired);
            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException(ApiMessages.ExpenseDescriptionRequired);
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException(ApiMessages.ExpenseStatusRequired);
        }

        private static ExpenseListItemDto MapListItem(ExpensesType expense)
        {
            var branchNames = expense.AllBranches
                ? new List<string>()
                : expense.ExpenseBranches
                    .Where(eb => eb.Branch != null && !eb.Branch.IsDeleted)
                    .Select(eb => eb.Branch.Name)
                    .OrderBy(n => n)
                    .ToList();

            return new ExpenseListItemDto
            {
                Id = expense.ExpensesTypeId,
                Name = expense.ExpensesTypeName,
                Description = expense.Description,
                AllBranches = expense.AllBranches,
                BranchNames = branchNames,
                Status = expense.Status
            };
        }

        private static ExpenseDetailDto MapDetail(ExpensesType expense)
        {
            var branches = expense.AllBranches
                ? new List<ExpenseBranchItemDto>()
                : expense.ExpenseBranches
                    .Where(eb => eb.Branch != null && !eb.Branch.IsDeleted)
                    .Select(eb => new ExpenseBranchItemDto
                    {
                        Id = eb.BranchId,
                        Name = eb.Branch.Name
                    })
                    .OrderBy(b => b.Name)
                    .ToList();

            return new ExpenseDetailDto
            {
                Id = expense.ExpensesTypeId,
                Name = expense.ExpensesTypeName,
                Description = expense.Description,
                AllBranches = expense.AllBranches,
                Branches = branches,
                Status = expense.Status
            };
        }
    }
}
