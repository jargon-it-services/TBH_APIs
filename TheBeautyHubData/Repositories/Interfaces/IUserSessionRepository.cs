using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<UserSession> InsertAsync(UserSession session);
        Task<UserSession> UpdateAsync(UserSession session);
        Task<int> RevokeAsync(Guid sessionId, string? revocationReason = null);
        Task<int> DeleteAsync(Guid sessionId);
        Task<UserSession?> GetByIdAsync(Guid sessionId);
        Task<IEnumerable<UserSession>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserSession>> GetActiveSessionsAsync(Guid userId);
    }
}
