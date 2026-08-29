using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Auth
{
    public sealed class AuthCenterUserLookup : IAuthCenterUserLookup
    {
        private readonly HttpClient _httpClient;
        private readonly AuthCenterOptions _options;
        private readonly ICurrentUser _currentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthCenterUserLookup> _logger;

        public AuthCenterUserLookup(
            HttpClient httpClient,
            IOptions<AuthCenterOptions> options,
            ICurrentUser currentUser,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthCenterUserLookup> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _currentUser = currentUser;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Guid?> ResolveCurrentUserIdAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUser.UserId != Guid.Empty)
                return _currentUser.UserId;

            return await FetchProfileUserIdAsync(cancellationToken);
        }

        public async Task<Guid?> ResolveUserIdAsync(
            string? email,
            string? username,
            CancellationToken cancellationToken = default)
        {
            var tokenUserId = _currentUser.UserId != Guid.Empty
                ? _currentUser.UserId
                : await FetchProfileUserIdAsync(cancellationToken);

            if (MatchesCurrentUser(email, username) && tokenUserId.HasValue)
                return tokenUserId;

            var fromAccount = await FindInAccountUsersAsync(email, username, cancellationToken);
            if (fromAccount.HasValue)
                return fromAccount;

            return null;
        }

        private bool MatchesCurrentUser(string? email, string? username)
        {
            if (!string.IsNullOrWhiteSpace(email) &&
                !string.IsNullOrWhiteSpace(_currentUser.Email) &&
                string.Equals(email.Trim(), _currentUser.Email.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(_currentUser.Email) &&
                string.Equals(username.Trim(), _currentUser.Email.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private async Task<Guid?> FetchProfileUserIdAsync(CancellationToken cancellationToken)
        {
            var token = GetAccessToken();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(_options.BaseUrl))
                return null;

            var url = $"{_options.BaseUrl.TrimEnd('/')}{_options.ProfilePath}";
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return null;

                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                if (!document.RootElement.TryGetProperty("data", out var data))
                    return null;

                if (data.TryGetProperty("user_info", out var userInfo) || data.TryGetProperty("userInfo", out userInfo))
                    return ReadGuid(userInfo, "id") ?? ReadGuid(userInfo, "Id");

                return ReadGuid(data, "userId") ?? ReadGuid(data, "UserId") ?? ReadGuid(data, "id");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not read AuthCenter user id from {Url}", url);
                return null;
            }
        }

        private async Task<Guid?> FindInAccountUsersAsync(string? email, string? username, CancellationToken cancellationToken)
        {
            var token = GetAccessToken();
            if (string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(_options.BaseUrl) ||
                _currentUser.AccountId == Guid.Empty)
            {
                return null;
            }

            var url = $"{_options.BaseUrl.TrimEnd('/')}/api/accounts/{_currentUser.AccountId}/users";
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return null;

                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                if (!document.RootElement.TryGetProperty("data", out var data))
                    return null;

                var users = data.ValueKind == JsonValueKind.Array
                    ? data
                    : data.TryGetProperty("users", out var nested) ? nested : default;

                if (users.ValueKind != JsonValueKind.Array)
                    return null;

                foreach (var user in users.EnumerateArray())
                {
                    var userEmail = ReadString(user, "email") ?? ReadString(user, "Email");
                    var userName = ReadString(user, "userName")
                        ?? ReadString(user, "user_name")
                        ?? ReadString(user, "UserName");
                    var id = ReadGuid(user, "id") ?? ReadGuid(user, "Id") ?? ReadGuid(user, "userId");
                    if (!id.HasValue)
                        continue;

                    if (!string.IsNullOrWhiteSpace(email) &&
                        string.Equals(userEmail?.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return id;
                    }

                    if (!string.IsNullOrWhiteSpace(username) &&
                        string.Equals(userName?.Trim(), username.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not look up AuthCenter users at {Url}", url);
            }

            return null;
        }

        private string? GetAccessToken()
        {
            var http = _httpContextAccessor.HttpContext;
            if (http == null)
                return null;

            if (http.Items.TryGetValue(AuthCenterClaimTypes.RawTokenItemKey, out var stored) && stored is string raw && !string.IsNullOrWhiteSpace(raw))
                return raw;

            var header = http.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(header))
                return null;

            const string bearer = "Bearer ";
            return header.StartsWith(bearer, StringComparison.OrdinalIgnoreCase)
                ? header[bearer.Length..].Trim()
                : header.Trim();
        }

        private static string? ReadString(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.String)
                return null;
            return value.GetString();
        }

        private static Guid? ReadGuid(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var value))
                return null;

            if (value.ValueKind == JsonValueKind.String && Guid.TryParse(value.GetString(), out var fromString))
                return fromString;

            if (value.ValueKind == JsonValueKind.Object)
                return null;

            try
            {
                if (value.TryGetGuid(out var guid))
                    return guid;
            }
            catch (InvalidOperationException)
            {
            }

            return null;
        }
    }
}
