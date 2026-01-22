using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        {
            var entity = _mapper.Map<Transaction>(dto);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<TransactionDto>(result);
        }

        public async Task<TransactionDto> UpdateAsync(Guid transactionId, UpdateTransactionDto dto)
        {
            var existing = await _repository.GetByIdAsync(transactionId);
            if (existing == null)
                throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");

            existing.Status = dto.Status;
            existing.TotalAmount = dto.TotalAmount;
            existing.PostedDate = dto.PostedDate;
            existing.TransactionDate = dto.TransactionDate;
            existing.CheckInTime = dto.CheckInTime;
            existing.CheckOutTime = dto.CheckOutTime;

            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<TransactionDto>(result);
        }

        public async Task<bool> DeleteAsync(Guid transactionId)
        {
            var result = await _repository.DeleteAsync(transactionId);
            return result > 0;
        }

        public async Task<TransactionDto?> GetByIdAsync(Guid transactionId)
        {
            var entity = await _repository.GetByIdAsync(transactionId);
            return entity == null ? null : _mapper.Map<TransactionDto>(entity);
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(entities);
        }

        public async Task<IEnumerable<TransactionDto>> GetByAccountIdAsync(Guid accountId)
        {
            var entities = await _repository.GetByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<TransactionDto>>(entities);
        }

        public async Task<IEnumerable<TransactionDto>> GetByFirmIdAsync(Guid firmId)
        {
            var entities = await _repository.GetByFirmIdAsync(firmId);
            return _mapper.Map<IEnumerable<TransactionDto>>(entities);
        }
    }
}
