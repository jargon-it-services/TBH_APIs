using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// Token validation only. Login, register, and password flows live in AuthCenter.
    /// </summary>
    [ApiController]
    [Route("api/token")]
    [Produces("application/json")]
    public class TokenController : ControllerBase
    {
        private readonly IAccessTokenService _accessTokenService;

        public TokenController(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        /// <summary>
        /// Validates the AuthCenter access token from the Authorization header.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate()
        {
            var token = _accessTokenService.ReadTokenFromRequest(Request);
            var result = await _accessTokenService.ValidateAsync(token, HttpContext.RequestAborted);

            return Ok(new ApiStatusResponse<TokenValidateDataResponse>
            {
                Status = result.IsValid,
                Data = new TokenValidateDataResponse
                {
                    IsValid = result.IsValid,
                    UserId = result.UserId,
                    AccountId = result.AccountId,
                    SessionId = result.SessionId,
                    ApplicationId = result.ApplicationId,
                    Email = result.Email,
                    Name = result.Name,
                    Roles = result.Roles,
                    Permissions = result.Permissions
                },
                Message = result.IsValid
                    ? ApiMessages.AuthTokenValid
                    : (result.Message ?? ApiMessages.AuthTokenInvalid)
            });
        }
    }

    public class TokenValidateDataResponse
    {
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("user_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? UserId { get; set; }

        [JsonPropertyName("account_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? AccountId { get; set; }

        [JsonPropertyName("session_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? SessionId { get; set; }

        [JsonPropertyName("application_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? ApplicationId { get; set; }

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Email { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();

        [JsonPropertyName("permissions")]
        public List<string> Permissions { get; set; } = new();
    }
}
