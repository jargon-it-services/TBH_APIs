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
    /// Service implementation for TransactionRules business logic.
    /// Handles validation and business rules for transaction rules operations.
    /// </summary>
    public class TransactionRulesService : ITransactionRulesService
    {
        private readonly ITransactionRulesRepository _transactionRulesRepository;
        private readonly IMapper _mapper;

        public TransactionRulesService(ITransactionRulesRepository transactionRulesRepository, IMapper mapper)
        {
            _transactionRulesRepository = transactionRulesRepository;
            _mapper = mapper;
        }

        public async Task<TransactionRulesDto> CreateTransactionRulesAsync(CreateTransactionRulesDto createTransactionRulesDto)
        {
            if (createTransactionRulesDto == null)
                throw new ArgumentNullException(nameof(createTransactionRulesDto));

            if (string.IsNullOrWhiteSpace(createTransactionRulesDto.RuleName))
                throw new ArgumentException("Rule name is required.");

            var transactionRules = _mapper.Map<TransactionRules>(createTransactionRulesDto);
            var createdTransactionRules = await _transactionRulesRepository.InsertTransactionRulesAsync(transactionRules);
            return _mapper.Map<TransactionRulesDto>(createdTransactionRules);
        }

        public async Task<TransactionRulesDto> UpdateTransactionRulesAsync(UpdateTransactionRulesDto updateTransactionRulesDto)
        {
            if (updateTransactionRulesDto == null)
                throw new ArgumentNullException(nameof(updateTransactionRulesDto));

            if (string.IsNullOrWhiteSpace(updateTransactionRulesDto.RuleName))
                throw new ArgumentException("Rule name is required.");

            var existingTransactionRules = await _transactionRulesRepository.GetTransactionRulesByIdAsync(updateTransactionRulesDto.TransactionRuleId);
            if (existingTransactionRules == null)
                throw new KeyNotFoundException($"Transaction rule with ID {updateTransactionRulesDto.TransactionRuleId} not found.");

            var transactionRules = _mapper.Map<TransactionRules>(updateTransactionRulesDto);
            var updatedTransactionRules = await _transactionRulesRepository.UpdateTransactionRulesAsync(transactionRules);
            return _mapper.Map<TransactionRulesDto>(updatedTransactionRules);
        }

        public async Task<bool> DeleteTransactionRulesAsync(Guid transactionRuleId)
        {
            var existingTransactionRules = await _transactionRulesRepository.GetTransactionRulesByIdAsync(transactionRuleId);
            if (existingTransactionRules == null)
                throw new KeyNotFoundException($"Transaction rule with ID {transactionRuleId} not found.");

            var result = await _transactionRulesRepository.DeleteTransactionRulesAsync(transactionRuleId);
            return result > 0;
        }

        public async Task<TransactionRulesDto?> GetTransactionRulesByIdAsync(Guid transactionRuleId)
        {
            var transactionRules = await _transactionRulesRepository.GetTransactionRulesByIdAsync(transactionRuleId);
            return transactionRules != null ? _mapper.Map<TransactionRulesDto>(transactionRules) : null;
        }

        public async Task<IEnumerable<TransactionRulesDto>> GetTransactionRulesByAccountIdAsync(Guid accountId)
        {
            var transactionRules = await _transactionRulesRepository.GetTransactionRulesByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<TransactionRulesDto>>(transactionRules);
        }

        public async Task<IEnumerable<TransactionRulesDto>> GetAllTransactionRulesAsync()
        {
            var transactionRules = await _transactionRulesRepository.GetAllTransactionRulesAsync();
            return _mapper.Map<IEnumerable<TransactionRulesDto>>(transactionRules);
        }
    }
}
