using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Helpers;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/staff")]
    [Produces("application/json")]
    public class StaffController : ControllerBase
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IStaffService _staffService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly StaffFileStorage _fileStorage;
        private readonly ICurrentUser _currentUser;
        private readonly IAuthCenterUserLookup _authCenterUsers;

        public StaffController(
            IStaffService staffService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            StaffFileStorage fileStorage,
            ICurrentUser currentUser,
            IAuthCenterUserLookup authCenterUsers)
        {
            _staffService = staffService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _currentUser = currentUser;
            _authCenterUsers = authCenterUsers;
        }

        /// <summary>API_031 Fetch Staff Form Config</summary>
        [HttpGet("form-config")]
        public async Task<IActionResult> GetFormConfig()
        {
            try
            {
                var config = await _staffService.GetFormConfigAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<StaffFormConfigDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffFormConfigFetched,
                    Data = _mapper.Map<StaffFormConfigDataResponse>(config)
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.StaffFormConfigFailed));
            }
        }

        /// <summary>API_032 Fetch Staff List</summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var staff = await _staffService.GetListAsync(_currentUser.AccountId);
                var items = _mapper.Map<List<StaffListItemResponse>>(staff);
                foreach (var item in items)
                    item.Photo = ToAbsoluteUrl(item.Photo);

                return Ok(new ApiStatusResponse<StaffListDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffListFetched,
                    Data = new StaffListDataResponse { Staff = items }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.StaffListFailed));
            }
        }

        /// <summary>API_034 Fetch Next Employee Code</summary>
        [HttpGet("next-employee-code")]
        public async Task<IActionResult> GetNextEmployeeCode()
        {
            try
            {
                var code = await _staffService.GetNextEmployeeCodeAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<NextEmployeeCodeDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffNextCodeFetched,
                    Data = new NextEmployeeCodeDataResponse { EmployeeCode = code }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return Ok(new ApiStatusResponse<NextEmployeeCodeDataResponse>
                {
                    Status = true,
                    Data = new NextEmployeeCodeDataResponse { EmployeeCode = null }
                });
            }
        }

        /// <summary>API_033 Fetch Staff Detail</summary>
        [HttpGet("{userId:guid}/details")]
        public async Task<IActionResult> GetDetails(Guid userId)
        {
            try
            {
                var detail = await _staffService.GetDetailsAsync(userId, _currentUser.AccountId);
                if (detail == null)
                    return NotFound(Fail(ApiMessages.StaffNotFound));

                var response = _mapper.Map<StaffDetailResponse>(detail);
                response.Photo = ToAbsoluteUrl(response.Photo);
                response.AadhaarCardUrl = ToAbsoluteUrl(response.AadhaarCardUrl);

                return Ok(new ApiStatusResponse<StaffDetailResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffDetailsFetched,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.StaffDetailsFailed));
            }
        }

        /// <summary>API_035 Create Staff. Multipart form (photo / aadhaar files) or JSON with the same field names.</summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> Create([FromForm] SaveStaffRequest request)
            => CreateCoreAsync(request);

        [HttpPost]
        [Consumes("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> CreateFromJson([FromBody] SaveStaffRequest request)
            => CreateCoreAsync(request);

        /// <summary>API_036 Update Staff. Same payload as create.</summary>
        [HttpPost("{userId:guid}")]
        [Consumes("multipart/form-data")]
        public Task<IActionResult> Update(Guid userId, [FromForm] SaveStaffRequest request)
            => UpdateCoreAsync(userId, request);

        [HttpPost("{userId:guid}")]
        [Consumes("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> UpdateFromJson(Guid userId, [FromBody] SaveStaffRequest request)
            => UpdateCoreAsync(userId, request);

        private async Task<IActionResult> CreateCoreAsync(SaveStaffRequest request)
        {
            try
            {
                request = await BindSaveRequestAsync(request);
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? photoPath = null;
                string? aadhaarPath = null;
                if (request.Photo != null)
                    photoPath = await _fileStorage.SavePhotoAsync(request.Photo);
                if (request.AadhaarCard != null)
                    aadhaarPath = await _fileStorage.SaveAadhaarAsync(request.AadhaarCard);

                await _staffService.CreateAsync(await MapSaveDtoAsync(request, photoPath, aadhaarPath));

                return Ok(new ApiStatusResponse<StaffSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffCreated,
                    Data = new StaffSavedDataResponse { Saved = true }
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
                return StatusCode(500, Fail(ApiMessages.StaffCreateFailed));
            }
        }

        private async Task<IActionResult> UpdateCoreAsync(Guid userId, SaveStaffRequest request)
        {
            try
            {
                var existing = await _staffService.GetDetailsAsync(userId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.StaffNotFound));

                request = await BindSaveRequestAsync(request);
                if (IsStatusOnlyUpdate(request))
                {
                    await _staffService.UpdateStatusAsync(userId, _currentUser.AccountId, request.Status ?? string.Empty);
                    return Ok(new ApiStatusResponse<StaffSavedDataResponse>
                    {
                        Status = true,
                        Message = ApiMessages.StaffUpdated,
                        Data = new StaffSavedDataResponse { Saved = true }
                    });
                }

                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? photoPath = null;
                string? aadhaarPath = null;
                if (request.Photo != null)
                    photoPath = await _fileStorage.SavePhotoAsync(request.Photo);
                if (request.AadhaarCard != null)
                    aadhaarPath = await _fileStorage.SaveAadhaarAsync(request.AadhaarCard);

                await _staffService.UpdateAsync(userId, await MapSaveDtoAsync(request, photoPath, aadhaarPath));

                if (request.RemovePhoto || photoPath != null)
                    _fileStorage.DeleteIfLocal(existing.Photo);
                if (request.RemoveAadhaarCard || aadhaarPath != null)
                    _fileStorage.DeleteIfLocal(existing.AadhaarCardUrl);

                return Ok(new ApiStatusResponse<StaffSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffUpdated,
                    Data = new StaffSavedDataResponse { Saved = true }
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
                return StatusCode(500, Fail(ApiMessages.StaffUpdateFailed));
            }
        }

        /// <summary>API_037 Delete Staff</summary>
        [HttpPost("{userId:guid}/delete")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try
            {
                var existing = await _staffService.GetDetailsAsync(userId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.StaffNotFound));

                await _staffService.DeleteAsync(userId, _currentUser.AccountId);
                _fileStorage.DeleteIfLocal(existing.Photo);
                _fileStorage.DeleteIfLocal(existing.AadhaarCardUrl);

                return Ok(new ApiStatusResponse<StaffDeletedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.StaffDeleted,
                    Data = new StaffDeletedDataResponse { Deleted = true }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.StaffDeleteFailed));
            }
        }

        private async Task<SaveStaffDto> MapSaveDtoAsync(SaveStaffRequest request, string? photoPath, string? aadhaarPath)
        {
            var createdBy = await _authCenterUsers.ResolveCurrentUserIdAsync() ?? _currentUser.UserId;
            Guid? staffUserId = null;
            if (request.AllowAppLogin == true)
                staffUserId = await _authCenterUsers.ResolveUserIdAsync(request.Email, request.Username);

            return new SaveStaffDto
            {
                AccountId = _currentUser.AccountId,
                CreatedBy = createdBy == Guid.Empty ? null : createdBy,
                UserId = staffUserId,
                FullName = request.FullName ?? string.Empty,
                Mobile = request.Mobile ?? string.Empty,
                Email = request.Email ?? string.Empty,
                Gender = request.Gender ?? string.Empty,
                AadhaarNumber = request.AadhaarNumber ?? string.Empty,
                EmployeeCode = request.EmployeeCode,
                JoiningDate = request.JoiningDate,
                Designation = request.Designation ?? string.Empty,
                Specialist = request.Specialist ?? string.Empty,
                BranchId = request.BranchId ?? Guid.Empty,
                SalaryRuleId = request.SalaryRuleId ?? Guid.Empty,
                Status = request.Status ?? string.Empty,
                AllowAppLogin = request.AllowAppLogin ?? false,
                AppRole = request.AppRole,
                Username = request.Username,
                Photo = photoPath,
                AadhaarCardUrl = aadhaarPath,
                RemovePhoto = request.RemovePhoto,
                RemoveAadhaarCard = request.RemoveAadhaarCard,
                HasNewPhoto = photoPath != null,
                HasNewAadhaarCard = aadhaarPath != null
            };
        }

        private async Task<SaveStaffRequest> BindSaveRequestAsync(SaveStaffRequest? bound)
        {
            if (bound != null && HasAnyStaffField(bound))
                return bound;

            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync();
                return new SaveStaffRequest
                {
                    FullName = form["full_name"].ToString(),
                    Mobile = form["mobile"].ToString(),
                    Email = form["email"].ToString(),
                    Gender = form["gender"].ToString(),
                    AadhaarNumber = form["aadhaar_number"].ToString(),
                    EmployeeCode = NullIfEmpty(form["employee_code"]),
                    JoiningDate = NullIfEmpty(form["joining_date"]),
                    Designation = form["designation"].ToString(),
                    Specialist = form["specialist"].ToString(),
                    BranchId = ParseGuid(form["branch_id"]),
                    SalaryRuleId = ParseGuid(form["salary_rule_id"]),
                    Status = NullIfEmpty(form["status"]),
                    AllowAppLogin = FormHas(form, "allow_app_login") ? ParseBool(form["allow_app_login"]) : null,
                    AppRole = NullIfEmpty(form["app_role"]),
                    Username = NullIfEmpty(form["username"]),
                    RemovePhoto = ParseBool(form["remove_photo"]),
                    RemoveAadhaarCard = ParseBool(form["remove_aadhaar_card"]),
                    Photo = form.Files.GetFile("photo"),
                    AadhaarCard = form.Files.GetFile("aadhaar_card")
                };
            }

            using var reader = new StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException(ApiMessages.RequestBodyRequired);

            return JsonSerializer.Deserialize<SaveStaffRequest>(json, JsonOptions)
                ?? throw new ArgumentException(ApiMessages.InvalidRequestBody);
        }

        private static bool IsStatusOnlyUpdate(SaveStaffRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.Status)
                && string.IsNullOrWhiteSpace(request.FullName)
                && string.IsNullOrWhiteSpace(request.Mobile)
                && string.IsNullOrWhiteSpace(request.Email)
                && string.IsNullOrWhiteSpace(request.Gender)
                && string.IsNullOrWhiteSpace(request.AadhaarNumber)
                && string.IsNullOrWhiteSpace(request.Designation)
                && string.IsNullOrWhiteSpace(request.Specialist)
                && request.BranchId == null
                && request.SalaryRuleId == null
                && request.AllowAppLogin == null
                && request.Photo == null
                && request.AadhaarCard == null;
        }

        private static bool HasAnyStaffField(SaveStaffRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.FullName)
                || !string.IsNullOrWhiteSpace(request.Status)
                || !string.IsNullOrWhiteSpace(request.Mobile)
                || !string.IsNullOrWhiteSpace(request.Email)
                || !string.IsNullOrWhiteSpace(request.Gender)
                || request.BranchId != null
                || request.SalaryRuleId != null
                || request.AllowAppLogin != null
                || request.Photo != null
                || request.AadhaarCard != null
                || request.RemovePhoto
                || request.RemoveAadhaarCard;
        }

        private static bool FormHas(Microsoft.AspNetCore.Http.IFormCollection form, string key)
            => form.ContainsKey(key);

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

        private static string? NullIfEmpty(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value;

        private static Guid? ParseGuid(string? value) =>
            Guid.TryParse(value, out var parsed) ? parsed : null;

        private static bool ParseBool(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return value.Equals("true", StringComparison.OrdinalIgnoreCase)
                || value.Equals("1")
                || value.Equals("on", StringComparison.OrdinalIgnoreCase);
        }
    }
}
