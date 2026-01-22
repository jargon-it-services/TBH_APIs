using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IUserSessionService
    {
        Task<UserSessionDto> CreateAsync(CreateUserSessionDto dto);
        Task<UserSessionDto> UpdateAsync(Guid sessionId, UpdateUserSessionDto dto);
        Task<bool> RevokeAsync(Guid sessionId, string? revocationReason = null);
        Task<bool> DeleteAsync(Guid sessionId);
        Task<UserSessionDto?> GetByIdAsync(Guid sessionId);
        Task<IEnumerable<UserSessionDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserSessionDto>> GetActiveSessionsAsync(Guid userId);
    }
}
