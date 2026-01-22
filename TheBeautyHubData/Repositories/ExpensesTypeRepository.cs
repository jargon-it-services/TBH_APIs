using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    /// <summary>
    /// Repository implementation for ExpensesType entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class ExpensesTypeRepository : IExpensesTypeRepository
    {
        private readonly BeautyHubDbContext _context;

        public ExpensesTypeRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<ExpensesType> InsertExpensesTypeAsync(ExpensesType expensesType)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountId", expensesType.AccountId),
                new SqlParameter("@ExpensesTypeName", expensesType.ExpensesTypeName),
                new SqlParameter("@CreatedBy", (object?)expensesType.CreatedBy ?? DBNull.Value),
                new SqlParameter("@FirmId", (object?)expensesType.FirmId ?? DBNull.Value)
            };

            var result = await _context.ExpensesTypes
                .FromSqlRaw("EXEC usp_Insert_ExpensesType @AccountId, @ExpensesTypeName, @CreatedBy, @FirmId", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? expensesType;
        }

        public async Task<ExpensesType> UpdateExpensesTypeAsync(ExpensesType expensesType)
        {
            var parameters = new[]
            {
                new SqlParameter("@ExpensesTypeId", expensesType.ExpensesTypeId),
                new SqlParameter("@ExpensesTypeName", expensesType.ExpensesTypeName),
                new SqlParameter("@FirmId", (object?)expensesType.FirmId ?? DBNull.Value)
            };

            var result = await _context.ExpensesTypes
                .FromSqlRaw("EXEC usp_Update_ExpensesType @ExpensesTypeId, @ExpensesTypeName, @FirmId", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? expensesType;
        }

        public async Task<int> DeleteExpensesTypeAsync(Guid expensesTypeId)
        {
            var parameter = new SqlParameter("@ExpensesTypeId", expensesTypeId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_ExpensesType @ExpensesTypeId", parameter);
        }

        public async Task<ExpensesType?> GetExpensesTypeByIdAsync(Guid expensesTypeId)
        {
            var parameter = new SqlParameter("@ExpensesTypeId", expensesTypeId);
            var result = await _context.ExpensesTypes
                .FromSqlRaw("EXEC usp_Get_ExpensesTypeById @ExpensesTypeId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<ExpensesType>> GetExpensesTypesByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.ExpensesTypes
                .FromSqlRaw("EXEC usp_Get_ExpensesTypesByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpensesType>> GetAllExpensesTypesAsync()
        {
            return await _context.ExpensesTypes
                .FromSqlRaw("EXEC usp_Get_AllExpensesTypes")
                .ToListAsync();
        }
    }
}
