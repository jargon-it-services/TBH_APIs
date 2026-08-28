using System.Threading;
using System.Threading.Tasks;

namespace TheBeautyHubAPI.Auth
{
    public interface IAuthCenterTokenValidator
    {
        Task<AuthCenterValidateResult> ValidateAsync(string accessToken, CancellationToken cancellationToken = default);
    }
}
