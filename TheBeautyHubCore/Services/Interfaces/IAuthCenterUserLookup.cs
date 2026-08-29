using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheBeautyHubCore.Services.Interfaces
{
    /// <summary>
    /// Resolves AuthCenter user ids. Token claims are preferred; AuthCenter is called when needed.
    /// </summary>
    public interface IAuthCenterUserLookup
    {
        Task<Guid?> ResolveCurrentUserIdAsync(CancellationToken cancellationToken = default);

        Task<Guid?> ResolveUserIdAsync(
            string? email,
            string? username,
            CancellationToken cancellationToken = default);
    }
}
