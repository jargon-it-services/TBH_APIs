using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TheBeautyHubAPI.Auth
{
    public sealed class AuthCenterTokenValidator : IAuthCenterTokenValidator
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly AuthCenterOptions _options;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AuthCenterTokenValidator> _logger;

        public AuthCenterTokenValidator(
            HttpClient httpClient,
            IOptions<AuthCenterOptions> options,
            IMemoryCache cache,
            ILogger<AuthCenterTokenValidator> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _cache = cache;
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

            try
            {
                var url = $"{_options.BaseUrl.TrimEnd('/')}{_options.ValidatePath}";
                using var response = await _httpClient.PostAsJsonAsync(url, new
                {
                    token = accessToken,
                    app_name = _options.ApplicationName
                }, JsonOptions, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("AuthCenter validate returned {StatusCode}", (int)response.StatusCode);
                    return Invalid("AuthCenter rejected the token.");
                }

                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                var parsed = Parse(document.RootElement);

                if (parsed.IsValid)
                {
                    var cacheSeconds = Math.Max(1, _options.ValidateCacheSeconds);
                    _cache.Set(cacheKey, parsed, TimeSpan.FromSeconds(cacheSeconds));
                }

                return parsed;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "AuthCenter token validation failed");
                return Invalid("Unable to validate token with AuthCenter.");
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
            var raw = GetString(element, name);
            return Guid.TryParse(raw, out var id) ? id : null;
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
    }
}
