using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class TransactionDetailService : ITransactionDetailService
    {
        private readonly ITransactionDetailRepository _repository;
        private readonly IMapper _mapper;

        public TransactionDetailService(ITransactionDetailRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDetailDto> CreateAsync(CreateTransactionDetailDto dto)
        {
            var entity = _mapper.Map<TransactionDetail>(dto);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<TransactionDetailDto>(result);
        }

        public async Task<TransactionDetailDto> UpdateAsync(Guid transactionDetailsId, UpdateTransactionDetailDto dto)
        {
            var existing = await _repository.GetByIdAsync(transactionDetailsId);
            if (existing == null)
                throw new KeyNotFoundException($"Transaction detail with ID {transactionDetailsId} not found.");

            existing.TransactionTypeId = dto.TransactionTypeId;
            existing.ExpensesTypeId = dto.ExpensesTypeId;
            existing.ServiceId = dto.ServiceId;
            existing.Amount = dto.Amount;
            existing.IncentiveAmount = dto.IncentiveAmount;
            existing.TransactionRuleId = dto.TransactionRuleId;

            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<TransactionDetailDto>(result);
        }

        public async Task<bool> DeleteAsync(Guid transactionDetailsId)
        {
            var result = await _repository.DeleteAsync(transactionDetailsId);
            return result > 0;
        }

        public async Task<TransactionDetailDto?> GetByIdAsync(Guid transactionDetailsId)
        {
            var entity = await _repository.GetByIdAsync(transactionDetailsId);
            return entity == null ? null : _mapper.Map<TransactionDetailDto>(entity);
        }

        public async Task<IEnumerable<TransactionDetailDto>> GetByTransactionIdAsync(Guid transactionId)
        {
            var entities = await _repository.GetByTransactionIdAsync(transactionId);
            return _mapper.Map<IEnumerable<TransactionDetailDto>>(entities);
        }
    }
}
