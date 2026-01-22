using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class ReportForAccountService : IReportForAccountService
    {
        private readonly IReportForAccountRepository _repository;
        private readonly IMapper _mapper;

        public ReportForAccountService(IReportForAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReportForAccountDto> CreateAsync(CreateReportForAccountDto dto)
        {
            var entity = _mapper.Map<ReportForAccount>(dto);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<ReportForAccountDto>(result);
        }

        public async Task<ReportForAccountDto> UpdateAsync(Guid id, UpdateReportForAccountDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Report-for-account with ID {id} not found.");

            existing.IsActive = dto.IsActive;

            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<ReportForAccountDto>(result);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _repository.DeleteAsync(id);
            return result > 0;
        }

        public async Task<ReportForAccountDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ReportForAccountDto>(entity);
        }

        public async Task<IEnumerable<ReportForAccountDto>> GetByAccountIdAsync(Guid accountId)
        {
            var entities = await _repository.GetByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<ReportForAccountDto>>(entities);
        }
    }
}
