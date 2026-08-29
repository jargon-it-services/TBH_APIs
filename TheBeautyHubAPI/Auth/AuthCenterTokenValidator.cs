using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TheBeautyHubAPI.Auth
{
    public sealed class AuthCenterTokenValidator : IAuthCenterTokenValidator
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = null
        };

        private readonly HttpClient _httpClient;
        private readonly AuthCenterOptions _options;
        private readonly IMemoryCache _cache;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<AuthCenterTokenValidator> _logger;

        public AuthCenterTokenValidator(
            HttpClient httpClient,
            IOptions<AuthCenterOptions> options,
            IMemoryCache cache,
            IHostEnvironment environment,
            ILogger<AuthCenterTokenValidator> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _cache = cache;
            _environment = environment;
            _logger = logger;
        }

        public async Task<AuthCenterValidateResult> ValidateAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Invalid("Missing access token.");

            var cacheKey = "authcenter:validate:" + HashToken(accessToken);
            if (_cache.TryGetValue(cacheKey, out AuthCenterValidateResult? cached) && cached != null)
                return cached;

            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
                return Invalid("AuthCenter base URL is not configured.");

            var url = $"{_options.BaseUrl.TrimEnd('/')}{_options.ValidatePath}";
            try
            {
                using var response = await _httpClient.PostAsJsonAsync(
                    url,
                    new AuthCenterValidateRequest
                    {
                        Token = accessToken,
                        ApplicationName = _options.ApplicationName
                    },
                    JsonOptions,
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("AuthCenter validate returned {StatusCode} from {Url}", (int)response.StatusCode, url);
                    return Invalid("AuthCenter rejected the token.");
                }

                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                var parsed = Parse(document.RootElement);
                if (parsed.IsValid && !parsed.UserId.HasValue)
                    parsed.UserId = await FetchProfileUserIdAsync(accessToken, cancellationToken);

                if (parsed.IsValid)
                {
                    var cacheSeconds = Math.Max(1, _options.ValidateCacheSeconds);
                    _cache.Set(cacheKey, parsed, TimeSpan.FromSeconds(cacheSeconds));
                }

                return parsed;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(ex, "Could not reach AuthCenter validate at {Url}", url);
                if (_environment.IsDevelopment())
                {
                    var profileUserId = await FetchProfileUserIdAsync(accessToken, cancellationToken);
                    return new AuthCenterValidateResult { IsValid = true, UserId = profileUserId };
                }

                return Invalid($"Could not reach AuthCenter at '{url}'. Start AuthCenter or update AuthCenter:BaseUrl.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "AuthCenter token validation failed for {Url}", url);
                return Invalid("Unable to validate token with AuthCenter.");
            }
        }

        private async Task<Guid?> FetchProfileUserIdAsync(string accessToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.BaseUrl) || string.IsNullOrWhiteSpace(_options.ProfilePath))
                return null;

            var url = $"{_options.BaseUrl.TrimEnd('/')}{_options.ProfilePath}";
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return null;

                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                if (!document.RootElement.TryGetProperty("data", out var data))
                    return null;

                if (data.TryGetProperty("user_info", out var userInfo) || data.TryGetProperty("userInfo", out userInfo))
                    return GetGuid(userInfo, "id") ?? GetGuid(userInfo, "Id");

                return GetGuid(data, "userId") ?? GetGuid(data, "UserId");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not read AuthCenter profile user id from {Url}", url);
                return null;
            }
        }

        private static AuthCenterValidateResult Parse(JsonElement root)
        {
            if (!root.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Object)
                return Invalid("AuthCenter returned an unexpected response.");

            var isValid = GetBool(data, "isValid") || GetBool(data, "IsValid");
            if (!isValid)
                return Invalid("Token is not valid for The Beauty Hub.");

            return new AuthCenterValidateResult
            {
                IsValid = true,
                UserId = GetGuid(data, "userId") ?? GetGuid(data, "UserId"),
                Email = GetString(data, "email") ?? GetString(data, "Email"),
                Roles = GetStringList(data, "roles") ?? GetStringList(data, "Roles") ?? new List<string>(),
                Permissions = GetStringList(data, "permissions") ?? GetStringList(data, "Permissions") ?? new List<string>()
            };
        }

        private static AuthCenterValidateResult Invalid(string error)
        {
            return new AuthCenterValidateResult { IsValid = false, Error = error };
        }

        private static bool GetBool(JsonElement element, string name)
        {
            return element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.True;
        }

        private static string? GetString(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.String)
                return null;
            return value.GetString();
        }

        private static Guid? GetGuid(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var value))
                return null;

            if (value.ValueKind == JsonValueKind.String)
            {
                var raw = value.GetString();
                return Guid.TryParse(raw, out var id) ? id : null;
            }

            try
            {
                return value.TryGetGuid(out var guid) ? guid : null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private static List<string>? GetStringList(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Array)
                return null;

            var list = new List<string>();
            foreach (var item in value.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    var text = item.GetString();
                    if (!string.IsNullOrWhiteSpace(text))
                        list.Add(text);
                }
            }

            return list;
        }

        private static string HashToken(string token)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }

        private sealed class AuthCenterValidateRequest
        {
            [System.Text.Json.Serialization.JsonPropertyName("token")]
            public string Token { get; set; } = string.Empty;

            [System.Text.Json.Serialization.JsonPropertyName("app_name")]
            public string ApplicationName { get; set; } = string.Empty;
        }
    }
}
