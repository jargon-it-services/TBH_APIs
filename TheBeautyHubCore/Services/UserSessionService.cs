using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUserSessionRepository _repository;
        private readonly IMapper _mapper;

        public UserSessionService(IUserSessionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserSessionDto> CreateAsync(CreateUserSessionDto dto)
        {
            if (dto.RefreshTokenHash == null || dto.RefreshTokenHash.Length != 32)
                throw new ArgumentException("RefreshTokenHash must be exactly 32 bytes.");

            var entity = _mapper.Map<UserSession>(dto);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<UserSessionDto>(result);
        }

        public async Task<UserSessionDto> UpdateAsync(Guid sessionId, UpdateUserSessionDto dto)
        {
            var existing = await _repository.GetByIdAsync(sessionId);
            if (existing == null)
                throw new KeyNotFoundException($"User session with ID {sessionId} not found.");

            existing.LastSeenAt = dto.LastSeenAt ?? DateTime.UtcNow;

            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<UserSessionDto>(result);
        }

        public async Task<bool> RevokeAsync(Guid sessionId, string? revocationReason = null)
        {
            var result = await _repository.RevokeAsync(sessionId, revocationReason);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid sessionId)
        {
            var result = await _repository.DeleteAsync(sessionId);
            return result > 0;
        }

        public async Task<UserSessionDto?> GetByIdAsync(Guid sessionId)
        {
            var entity = await _repository.GetByIdAsync(sessionId);
            return entity == null ? null : _mapper.Map<UserSessionDto>(entity);
        }

        public async Task<IEnumerable<UserSessionDto>> GetByUserIdAsync(Guid userId)
        {
            var entities = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserSessionDto>>(entities);
        }

        public async Task<IEnumerable<UserSessionDto>> GetActiveSessionsAsync(Guid userId)
        {
            var entities = await _repository.GetActiveSessionsAsync(userId);
            return _mapper.Map<IEnumerable<UserSessionDto>>(entities);
        }
    }
}
