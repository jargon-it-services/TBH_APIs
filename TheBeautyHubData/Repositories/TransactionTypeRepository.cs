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
    /// Repository implementation for TransactionType entity.
    /// Uses stored procedures for all CRUD operations via EF Core.
    /// </summary>
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly BeautyHubDbContext _context;

        public TransactionTypeRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionType> InsertTransactionTypeAsync(TransactionType transactionType)
        {
            var parameter = new SqlParameter("@TransactionType", transactionType.Type);

            var result = await _context.TransactionTypes
                .FromSqlRaw("EXEC usp_Insert_TransactionType @TransactionType", parameter)
                .ToListAsync();

            return result.FirstOrDefault() ?? transactionType;
        }

        public async Task<TransactionType> UpdateTransactionTypeAsync(TransactionType transactionType)
        {
            var parameters = new[]
            {
                new SqlParameter("@TransactionTypeId", transactionType.TransactionTypeId),
                new SqlParameter("@TransactionType", transactionType.Type),
                new SqlParameter("@IsTransactionTypeActive", transactionType.IsTransactionTypeActive)
            };

            var result = await _context.TransactionTypes
                .FromSqlRaw("EXEC usp_Update_TransactionType @TransactionTypeId, @TransactionType, @IsTransactionTypeActive", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? transactionType;
        }

        public async Task<int> DeleteTransactionTypeAsync(Guid transactionTypeId)
        {
            var parameter = new SqlParameter("@TransactionTypeId", transactionTypeId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_TransactionType @TransactionTypeId", parameter);
        }

        public async Task<TransactionType?> GetTransactionTypeByIdAsync(Guid transactionTypeId)
        {
            var parameter = new SqlParameter("@TransactionTypeId", transactionTypeId);
            var result = await _context.TransactionTypes
                .FromSqlRaw("EXEC usp_Get_TransactionTypeById @TransactionTypeId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TransactionType>> GetAllTransactionTypesAsync()
        {
            return await _context.TransactionTypes
                .FromSqlRaw("EXEC usp_Get_AllTransactionTypes")
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionType>> GetActiveTransactionTypesAsync()
        {
            return await _context.TransactionTypes
                .FromSqlRaw("EXEC usp_Get_ActiveTransactionTypes")
                .ToListAsync();
        }
    }
}
