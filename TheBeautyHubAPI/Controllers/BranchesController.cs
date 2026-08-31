using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Helpers;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Parsing;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// Branch list, detail, create, and update endpoints   (API_021–API_024).
    /// Requires an AuthCenter Bearer access token.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/branches")]
    [Produces("application/json")]
    public class BranchesController : ControllerBase
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IBranchService _branchService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly BranchLogoStorage _logoStorage;
        private readonly ICurrentUser _currentUser;

        public BranchesController(
            IBranchService branchService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            BranchLogoStorage logoStorage,
            ICurrentUser currentUser)
        {
            _branchService = branchService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _logoStorage = logoStorage;
            _currentUser = currentUser;
        }

        /// <summary>
        /// API_021 Fetch Branches
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            try
            {
                var branches = await _branchService.GetBranchesAsync(_currentUser.AccountId);
                var items = _mapper.Map<List<BranchListItemResponse>>(branches);
                foreach (var item in items)
                    item.Logo = ToAbsoluteUrl(item.Logo);

                return Ok(new ApiStatusResponse<BranchListDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.BranchListFetched,
                    Data = new BranchListDataResponse { Branches = items }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.BranchListFailed));
            }
        }

        /// <summary>
        /// API_022 Fetch Branch Detail
        /// </summary>
        [HttpGet("{branchId:guid}/details")]
        public async Task<IActionResult> GetBranchDetails(Guid branchId)
        {
            try
            {
                var detail = await _branchService.GetBranchDetailsAsync(branchId, _currentUser.AccountId);
                if (detail == null)
                    return NotFound(Fail(ApiMessages.BranchNotFound));

                var response = _mapper.Map<BranchDetailResponse>(detail);
                response.Logo = ToAbsoluteUrl(response.Logo);
                if (response.Employees == null || response.Employees.Count == 0)
                {
                    response.Employees = null;
                }
                else
                {
                    foreach (var employee in response.Employees)
                        employee.Photo = ToAbsoluteUrl(employee.Photo);
                }

                return Ok(new ApiStatusResponse<BranchDetailResponse>
                {
                    Status = true,
                    Message = ApiMessages.BranchDetailsFetched,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.BranchDetailsFailed));
            }
        }

        /// <summary>
        /// API_023 Create Branch. Send multipart form (includes optional logo file) or JSON with the same field names.
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> CreateBranch([FromForm] SaveBranchRequest request)
            => CreateBranchCoreAsync(request);

        [HttpPost]
        [Consumes("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> CreateBranchFromJson([FromBody] SaveBranchRequest request)
            => CreateBranchCoreAsync(request);

        /// <summary>
        /// API_024 Update Branch. Same payload as create (multipart form or JSON).
        /// </summary>
        [HttpPost("{branchId:guid}")]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> UpdateBranch(Guid branchId, [FromForm] SaveBranchRequest request)
            => UpdateBranchCoreAsync(branchId, request);

        [HttpPost("{branchId:guid}")]
        [Consumes("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> UpdateBranchFromJson(Guid branchId, [FromBody] SaveBranchRequest request)
            => UpdateBranchCoreAsync(branchId, request);

        private async Task<IActionResult> CreateBranchCoreAsync(SaveBranchRequest request)
        {
            try
            {
                request = await BindSaveRequestAsync(request);
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? savedLogo = null;
                if (request.Logo != null)
                    savedLogo = await _logoStorage.SaveAsync(request.Logo);

                var dto = MapSaveDto(request, savedLogo, hasNewLogo: savedLogo != null);
                await _branchService.CreateBranchAsync(dto);

                return Ok(new ApiStatusResponse<BranchSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.BranchCreated,
                    Data = new BranchSavedDataResponse { Saved = true }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Fail(ex.Message));
            }
            catch (JsonException)
            {
                return BadRequest(Fail(ApiMessages.InvalidRequestBody));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.BranchCreateFailed));
            }
        }

        private async Task<IActionResult> UpdateBranchCoreAsync(Guid branchId, SaveBranchRequest request)
        {
            try
            {
                var existing = await _branchService.GetBranchDetailsAsync(branchId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.BranchNotFound));

                request = await BindSaveRequestAsync(request);
                if (IsStatusOnlyUpdate(request))
                {
                    await _branchService.UpdateStatusAsync(branchId, _currentUser.AccountId, request.Status ?? string.Empty);
                    return Ok(new ApiStatusResponse<BranchSavedDataResponse>
                    {
                        Status = true,
                        Message = ApiMessages.BranchUpdated,
                        Data = new BranchSavedDataResponse { Saved = true }
                    });
                }

                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? savedLogo = null;
                if (request.Logo != null)
                    savedLogo = await _logoStorage.SaveAsync(request.Logo);

                var dto = MapSaveDto(request, savedLogo, hasNewLogo: savedLogo != null);
                await _branchService.UpdateBranchAsync(branchId, _currentUser.AccountId, dto);

                if (request.RemoveLogo || savedLogo != null)
                    _logoStorage.DeleteIfLocal(existing.Logo);

                return Ok(new ApiStatusResponse<BranchSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.BranchUpdated,
                    Data = new BranchSavedDataResponse { Saved = true }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Fail(ex.Message));
            }
            catch (JsonException)
            {
                return BadRequest(Fail(ApiMessages.InvalidRequestBody));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.BranchUpdateFailed));
            }
        }

        private SaveBranchDto MapSaveDto(SaveBranchRequest request, string? logoPath, bool hasNewLogo)
        {
            return new SaveBranchDto
            {
                AccountId = _currentUser.AccountId,
                CreatedBy = _currentUser.UserId,
                Name = request.Name ?? string.Empty,
                AddressLine1 = request.AddressLine1 ?? string.Empty,
                AddressLine2 = request.AddressLine2,
                City = request.City ?? string.Empty,
                State = request.State ?? string.Empty,
                Pincode = request.Pincode ?? string.Empty,
                Mobile = request.Mobile ?? string.Empty,
                Email = request.Email ?? string.Empty,
                BranchType = request.BranchType ?? string.Empty,
                OpeningTime = request.OpeningTime ?? string.Empty,
                ClosingTime = request.ClosingTime ?? string.Empty,
                WeeklyOff = request.WeeklyOff ?? string.Empty,
                Status = request.Status ?? string.Empty,
                Services = MergeServiceIds(request),
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                MapsLink = request.MapsLink,
                Logo = logoPath,
                RemoveLogo = request.RemoveLogo,
                HasNewLogo = hasNewLogo
            };
        }

        private async Task<SaveBranchRequest> BindSaveRequestAsync(SaveBranchRequest? bound)
        {
            // JSON [FromBody] already consumed the stream. Do not re-read it for status-only payloads.
            if (bound != null && HasAnyBranchField(bound))
                return bound;

            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync();
                return new SaveBranchRequest
                {
                    Name = form["name"].ToString(),
                    AddressLine1 = form["address_line1"].ToString(),
                    AddressLine2 = NullIfEmpty(form["address_line2"]),
                    City = form["city"].ToString(),
                    State = form["state"].ToString(),
                    Pincode = form["pincode"].ToString(),
                    Mobile = form["mobile"].ToString(),
                    Email = form["email"].ToString(),
                    BranchType = form["branch_type"].ToString(),
                    OpeningTime = form["opening_time"].ToString(),
                    ClosingTime = form["closing_time"].ToString(),
                    WeeklyOff = form["weekly_off"].ToString(),
                    Status = form["status"].ToString(),
                    Services = FormHas(form, "services") ? ParseServiceIds(form["services"]) : null,
                    ServiceId = ParseGuid(form["service_id"]),
                    ServiceIds = FormHas(form, "service_ids") ? ParseServiceIds(form["service_ids"]) : null,
                    Latitude = ParseDecimal(form["latitude"]),
                    Longitude = ParseDecimal(form["longitude"]),
                    MapsLink = NullIfEmpty(form["maps_link"]),
                    RemoveLogo = ParseBool(form["remove_logo"]),
                    Logo = form.Files.GetFile("logo")
                };
            }

            using var reader = new StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException(ApiMessages.RequestBodyRequired);

            var request = JsonSerializer.Deserialize<SaveBranchRequest>(json, JsonOptions);
            if (request == null)
                throw new ArgumentException(ApiMessages.InvalidRequestBody);

            return request;
        }

        private static bool IsStatusOnlyUpdate(SaveBranchRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.Status)
                && string.IsNullOrWhiteSpace(request.Name)
                && string.IsNullOrWhiteSpace(request.AddressLine1)
                && string.IsNullOrWhiteSpace(request.City)
                && string.IsNullOrWhiteSpace(request.Mobile)
                && string.IsNullOrWhiteSpace(request.Email);
        }

        private static bool HasAnyBranchField(SaveBranchRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.Name)
                || !string.IsNullOrWhiteSpace(request.Status)
                || !string.IsNullOrWhiteSpace(request.AddressLine1)
                || !string.IsNullOrWhiteSpace(request.City)
                || !string.IsNullOrWhiteSpace(request.Mobile)
                || !string.IsNullOrWhiteSpace(request.Email)
                || request.Logo != null;
        }

        private string? ToAbsoluteUrl(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;
            if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return path;

            var relative = path.StartsWith('/') ? path : $"/{path}";
            return $"{Request.Scheme}://{Request.Host}{relative}";
        }

        private string GetModelStateError()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m))
                ?? ApiMessages.ValidationOccurred;
        }

        private static ApiStatusResponse<object> Fail(string message)
        {
            return new ApiStatusResponse<object>
            {
                Status = false,
                Data = null,
                Message = message
            };
        }

        private static string? NullIfEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static Guid? ParseGuid(string? value)
        {
            return Guid.TryParse(value, out var parsed) ? parsed : null;
        }

        private static decimal? ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : null;
        }

        private static bool ParseBool(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return value.Equals("true", StringComparison.OrdinalIgnoreCase)
                || value.Equals("1")
                || value.Equals("on", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsRealServiceId(Guid id) => id != Guid.Empty;

        private static List<Guid>? MergeServiceIds(SaveBranchRequest request)
        {
            var ids = new List<Guid>();
            if (request.Services != null)
                ids.AddRange(request.Services.Where(IsRealServiceId));
            if (request.ServiceIds != null)
                ids.AddRange(request.ServiceIds.Where(IsRealServiceId));
            if (request.ServiceId.HasValue && IsRealServiceId(request.ServiceId.Value))
                ids.Add(request.ServiceId.Value);

            return ids.Count == 0 ? null : ids.Distinct().ToList();
        }

        private static bool FormHas(IFormCollection form, string key)
        {
            return form.ContainsKey(key);
        }

        private static List<Guid> ParseServiceIds(Microsoft.Extensions.Primitives.StringValues values)
            => GuidListParser.ParseMany(values.Select(v => v));
    }
}
