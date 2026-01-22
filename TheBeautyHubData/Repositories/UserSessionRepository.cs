using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly BeautyHubDbContext _context;

        public UserSessionRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<UserSession> InsertAsync(UserSession session)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", session.UserId),
                new SqlParameter("@IpAddress", (object?)session.IpAddress ?? DBNull.Value),
                new SqlParameter("@UserAgent", (object?)session.UserAgent ?? DBNull.Value),
                new SqlParameter("@DeviceId", (object?)session.DeviceId ?? DBNull.Value),
                new SqlParameter("@AccessTokenJti", session.AccessTokenJti),
                new SqlParameter("@RefreshTokenHash", session.RefreshTokenHash),
                new SqlParameter("@RefreshTokenExpiresAt", session.RefreshTokenExpiresAt)
            };

            var result = await _context.UserSessions
                .FromSqlRaw("EXEC usp_Insert_UserSession @UserId, @IpAddress, @UserAgent, @DeviceId, @AccessTokenJti, @RefreshTokenHash, @RefreshTokenExpiresAt", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? session;
        }

        public async Task<UserSession> UpdateAsync(UserSession session)
        {
            var parameters = new[]
            {
                new SqlParameter("@SessionId", session.SessionId),
                new SqlParameter("@LastSeenAt", (object?)session.LastSeenAt ?? DBNull.Value)
            };

            var result = await _context.UserSessions
                .FromSqlRaw("EXEC usp_Update_UserSession @SessionId, @LastSeenAt", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? session;
        }

        public async Task<int> RevokeAsync(Guid sessionId, string? revocationReason = null)
        {
            var parameters = new[]
            {
                new SqlParameter("@SessionId", sessionId),
                new SqlParameter("@RevocationReason", (object?)revocationReason ?? DBNull.Value)
            };

            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Revoke_UserSession @SessionId, @RevocationReason", parameters);
        }

        public async Task<int> DeleteAsync(Guid sessionId)
        {
            var parameter = new SqlParameter("@SessionId", sessionId);
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_Delete_UserSession @SessionId", parameter);
        }

        public async Task<UserSession?> GetByIdAsync(Guid sessionId)
        {
            var parameter = new SqlParameter("@SessionId", sessionId);
            var result = await _context.UserSessions
                .FromSqlRaw("EXEC usp_Get_UserSessionById @SessionId", parameter)
                .ToListAsync();
            
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<UserSession>> GetByUserIdAsync(Guid userId)
        {
            var parameter = new SqlParameter("@UserId", userId);
            return await _context.UserSessions
                .FromSqlRaw("EXEC usp_Get_UserSessionsByUserId @UserId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSession>> GetActiveSessionsAsync(Guid userId)
        {
            var parameter = new SqlParameter("@UserId", userId);
            return await _context.UserSessions
                .FromSqlRaw("EXEC usp_Get_ActiveUserSessions @UserId", parameter)
                .ToListAsync();
        }
    }
}
