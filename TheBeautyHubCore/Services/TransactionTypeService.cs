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
    /// Service implementation for TransactionType business logic.
    /// Handles validation and business rules for transaction type operations.
    /// </summary>
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly IMapper _mapper;

        public TransactionTypeService(ITransactionTypeRepository transactionTypeRepository, IMapper mapper)
        {
            _transactionTypeRepository = transactionTypeRepository;
            _mapper = mapper;
        }

        public async Task<TransactionTypeDto> CreateTransactionTypeAsync(CreateTransactionTypeDto createTransactionTypeDto)
        {
            if (createTransactionTypeDto == null)
                throw new ArgumentNullException(nameof(createTransactionTypeDto));

            if (string.IsNullOrWhiteSpace(createTransactionTypeDto.Type))
                throw new ArgumentException("Transaction type is required.");

            var transactionType = _mapper.Map<TransactionType>(createTransactionTypeDto);
            var createdTransactionType = await _transactionTypeRepository.InsertTransactionTypeAsync(transactionType);
            return _mapper.Map<TransactionTypeDto>(createdTransactionType);
        }

        public async Task<TransactionTypeDto> UpdateTransactionTypeAsync(UpdateTransactionTypeDto updateTransactionTypeDto)
        {
            if (updateTransactionTypeDto == null)
                throw new ArgumentNullException(nameof(updateTransactionTypeDto));

            if (string.IsNullOrWhiteSpace(updateTransactionTypeDto.Type))
                throw new ArgumentException("Transaction type is required.");

            var existingTransactionType = await _transactionTypeRepository.GetTransactionTypeByIdAsync(updateTransactionTypeDto.TransactionTypeId);
            if (existingTransactionType == null)
                throw new KeyNotFoundException($"Transaction type with ID {updateTransactionTypeDto.TransactionTypeId} not found.");

            var transactionType = _mapper.Map<TransactionType>(updateTransactionTypeDto);
            var updatedTransactionType = await _transactionTypeRepository.UpdateTransactionTypeAsync(transactionType);
            return _mapper.Map<TransactionTypeDto>(updatedTransactionType);
        }

        public async Task<bool> DeleteTransactionTypeAsync(Guid transactionTypeId)
        {
            var existingTransactionType = await _transactionTypeRepository.GetTransactionTypeByIdAsync(transactionTypeId);
            if (existingTransactionType == null)
                throw new KeyNotFoundException($"Transaction type with ID {transactionTypeId} not found.");

            var result = await _transactionTypeRepository.DeleteTransactionTypeAsync(transactionTypeId);
            return result > 0;
        }

        public async Task<TransactionTypeDto?> GetTransactionTypeByIdAsync(Guid transactionTypeId)
        {
            var transactionType = await _transactionTypeRepository.GetTransactionTypeByIdAsync(transactionTypeId);
            return transactionType != null ? _mapper.Map<TransactionTypeDto>(transactionType) : null;
        }

        public async Task<IEnumerable<TransactionTypeDto>> GetAllTransactionTypesAsync()
        {
            var transactionTypes = await _transactionTypeRepository.GetAllTransactionTypesAsync();
            return _mapper.Map<IEnumerable<TransactionTypeDto>>(transactionTypes);
        }

        public async Task<IEnumerable<TransactionTypeDto>> GetActiveTransactionTypesAsync()
        {
            var transactionTypes = await _transactionTypeRepository.GetActiveTransactionTypesAsync();
            return _mapper.Map<IEnumerable<TransactionTypeDto>>(transactionTypes);
        }
    }
}
