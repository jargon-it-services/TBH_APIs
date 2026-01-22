using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReportDto> CreateAsync(CreateReportDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ReportName))
                throw new ArgumentException("Report name cannot be empty.");

            var entity = _mapper.Map<Report>(dto);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<ReportDto>(result);
        }

        public async Task<ReportDto> UpdateAsync(Guid reportId, UpdateReportDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ReportName))
                throw new ArgumentException("Report name cannot be empty.");

            var existing = await _repository.GetByIdAsync(reportId);
            if (existing == null)
                throw new KeyNotFoundException($"Report with ID {reportId} not found.");

            existing.ReportName = dto.ReportName;
            existing.IsActive = dto.IsActive;

            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<ReportDto>(result);
        }

        public async Task<bool> DeleteAsync(Guid reportId)
        {
            var result = await _repository.DeleteAsync(reportId);
            return result > 0;
        }

        public async Task<ReportDto?> GetByIdAsync(Guid reportId)
        {
            var entity = await _repository.GetByIdAsync(reportId);
            return entity == null ? null : _mapper.Map<ReportDto>(entity);
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReportDto>>(entities);
        }

        public async Task<IEnumerable<ReportDto>> GetActiveReportsAsync()
        {
            var entities = await _repository.GetActiveReportsAsync();
            return _mapper.Map<IEnumerable<ReportDto>>(entities);
        }
    }
}
