using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TheBeautyHubAPI.Auth
{
    public interface IAccessTokenService
    {
        string? ReadTokenFromRequest(HttpRequest request);
        Task<AccessTokenValidationResult> ValidateAsync(string? accessToken, CancellationToken cancellationToken = default);
    }
}
