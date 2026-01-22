using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    /// <summary>
    /// Service implementation for ExpensesType business logic.
    /// Handles validation and business rules for expenses type operations.
    /// </summary>
    public class ExpensesTypeService : IExpensesTypeService
    {
        private readonly IExpensesTypeRepository _expensesTypeRepository;
        private readonly IMapper _mapper;

        public ExpensesTypeService(IExpensesTypeRepository expensesTypeRepository, IMapper mapper)
        {
            _expensesTypeRepository = expensesTypeRepository;
            _mapper = mapper;
        }

        public async Task<ExpensesTypeDto> CreateExpensesTypeAsync(CreateExpensesTypeDto createExpensesTypeDto)
        {
            if (createExpensesTypeDto == null)
                throw new ArgumentNullException(nameof(createExpensesTypeDto));

            if (string.IsNullOrWhiteSpace(createExpensesTypeDto.ExpensesTypeName))
                throw new ArgumentException("Expenses type name is required.");

            var expensesType = _mapper.Map<ExpensesType>(createExpensesTypeDto);
            var createdExpensesType = await _expensesTypeRepository.InsertExpensesTypeAsync(expensesType);
            return _mapper.Map<ExpensesTypeDto>(createdExpensesType);
        }

        public async Task<ExpensesTypeDto> UpdateExpensesTypeAsync(UpdateExpensesTypeDto updateExpensesTypeDto)
        {
            if (updateExpensesTypeDto == null)
                throw new ArgumentNullException(nameof(updateExpensesTypeDto));

            if (string.IsNullOrWhiteSpace(updateExpensesTypeDto.ExpensesTypeName))
                throw new ArgumentException("Expenses type name is required.");

            var existingExpensesType = await _expensesTypeRepository.GetExpensesTypeByIdAsync(updateExpensesTypeDto.ExpensesTypeId);
            if (existingExpensesType == null)
                throw new KeyNotFoundException($"Expenses type with ID {updateExpensesTypeDto.ExpensesTypeId} not found.");

            var expensesType = _mapper.Map<ExpensesType>(updateExpensesTypeDto);
            var updatedExpensesType = await _expensesTypeRepository.UpdateExpensesTypeAsync(expensesType);
            return _mapper.Map<ExpensesTypeDto>(updatedExpensesType);
        }

        public async Task<bool> DeleteExpensesTypeAsync(Guid expensesTypeId)
        {
            var existingExpensesType = await _expensesTypeRepository.GetExpensesTypeByIdAsync(expensesTypeId);
            if (existingExpensesType == null)
                throw new KeyNotFoundException($"Expenses type with ID {expensesTypeId} not found.");

            var result = await _expensesTypeRepository.DeleteExpensesTypeAsync(expensesTypeId);
            return result > 0;
        }

        public async Task<ExpensesTypeDto?> GetExpensesTypeByIdAsync(Guid expensesTypeId)
        {
            var expensesType = await _expensesTypeRepository.GetExpensesTypeByIdAsync(expensesTypeId);
            return expensesType != null ? _mapper.Map<ExpensesTypeDto>(expensesType) : null;
        }

        public async Task<IEnumerable<ExpensesTypeDto>> GetExpensesTypesByAccountIdAsync(Guid accountId)
        {
            var expensesTypes = await _expensesTypeRepository.GetExpensesTypesByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<ExpensesTypeDto>>(expensesTypes);
        }

        public async Task<IEnumerable<ExpensesTypeDto>> GetAllExpensesTypesAsync()
        {
            var expensesTypes = await _expensesTypeRepository.GetAllExpensesTypesAsync();
            return _mapper.Map<IEnumerable<ExpensesTypeDto>>(expensesTypes);
        }
    }
}
