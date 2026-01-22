using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class ExceptionLogService : IExceptionLogService
    {
        private readonly IExceptionLogRepository _repository;
        private readonly IMapper _mapper;

        public ExceptionLogService(IExceptionLogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Logs an exception to the database. Use this method from controllers to log exceptions.
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        /// <param name="userId">Optional user ID associated with the exception</param>
        /// <param name="additionalInfo">Optional additional information about the exception context</param>
        /// <returns>The ID of the created log entry</returns>
        public async Task<long> LogExceptionAsync(Exception exception, Guid? userId = null, string? additionalInfo = null)
        {
            try
            {
                var dto = new CreateExceptionLogDto
                {
                    Type = exception.GetType().Name,
                    ErrorMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.ToString(),
                    AdditionalInfo = additionalInfo,
                    UserId = userId
                };

                var entity = _mapper.Map<ExceptionLog>(dto);
                var id = await _repository.InsertAsync(entity);
                return id;
            }
            catch
            {
                // Swallow any exceptions during logging to prevent cascading failures
                return 0;
            }
        }

        public async Task<long> CreateAsync(CreateExceptionLogDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new ArgumentException("Exception type cannot be empty.");

            if (string.IsNullOrWhiteSpace(dto.ErrorMessage))
                throw new ArgumentException("Error message cannot be empty.");

            var entity = _mapper.Map<ExceptionLog>(dto);
            var id = await _repository.InsertAsync(entity);
            return id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var result = await _repository.DeleteAsync(id);
            return result > 0;
        }

        public async Task<ExceptionLogDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ExceptionLogDto>(entity);
        }

        public async Task<IEnumerable<ExceptionLogDto>> GetAllAsync(int pageSize = 100, int pageNumber = 1)
        {
            var entities = await _repository.GetAllAsync(pageSize, pageNumber);
            return _mapper.Map<IEnumerable<ExceptionLogDto>>(entities);
        }

        public async Task<IEnumerable<ExceptionLogDto>> GetByUserIdAsync(Guid userId, int pageSize = 100, int pageNumber = 1)
        {
            var entities = await _repository.GetByUserIdAsync(userId, pageSize, pageNumber);
            return _mapper.Map<IEnumerable<ExceptionLogDto>>(entities);
        }

        public async Task<IEnumerable<ExceptionLogDto>> GetByTypeAsync(string type, int pageSize = 100, int pageNumber = 1)
        {
            var entities = await _repository.GetByTypeAsync(type, pageSize, pageNumber);
            return _mapper.Map<IEnumerable<ExceptionLogDto>>(entities);
        }

        public async Task<int> DeleteOldLogsAsync(int daysToKeep = 90)
        {
            return await _repository.DeleteOldLogsAsync(daysToKeep);
        }
    }
}
