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

        public StaffController(
            IStaffService staffService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            StaffFileStorage fileStorage,
            ICurrentUser currentUser)
        {
            _staffService = staffService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _currentUser = currentUser;
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
                    Message = ApiMessages.Staff.FormConfigFetched,
                    Data = _mapper.Map<StaffFormConfigDataResponse>(config)
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Staff.FormConfigFailed));
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
                    Message = ApiMessages.Staff.ListFetched,
                    Data = new StaffListDataResponse { Staff = items }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Staff.ListFailed));
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
                    Message = ApiMessages.Staff.NextCodeFetched,
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
                    return NotFound(Fail(ApiMessages.Staff.NotFound));

                var response = _mapper.Map<StaffDetailResponse>(detail);
                response.Photo = ToAbsoluteUrl(response.Photo);
                response.AadhaarCardUrl = ToAbsoluteUrl(response.AadhaarCardUrl);

                return Ok(new ApiStatusResponse<StaffDetailResponse>
                {
                    Status = true,
                    Message = ApiMessages.Staff.DetailsFetched,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Staff.DetailsFailed));
            }
        }

        /// <summary>API_035 Create Staff</summary>
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                var request = await BindSaveRequestAsync();
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? photoPath = null;
                string? aadhaarPath = null;
                if (request.Photo != null)
                    photoPath = await _fileStorage.SavePhotoAsync(request.Photo);
                if (request.AadhaarCard != null)
                    aadhaarPath = await _fileStorage.SaveAadhaarAsync(request.AadhaarCard);

                await _staffService.CreateAsync(MapSaveDto(request, photoPath, aadhaarPath));

                return Ok(new ApiStatusResponse<StaffSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.Staff.Created,
                    Data = new StaffSavedDataResponse { Saved = true }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Fail(ex.Message));
            }
            catch (JsonException)
            {
                return BadRequest(Fail(ApiMessages.Common.InvalidRequestBody));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Staff.CreateFailed));
            }
        }

        /// <summary>API_036 Update Staff</summary>
        [HttpPost("{userId:guid}")]
        public async Task<IActionResult> Update(Guid userId)
        {
            try
            {
                var existing = await _staffService.GetDetailsAsync(userId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.Staff.NotFound));

                var request = await BindSaveRequestAsync();
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? photoPath = null;
                string? aadhaarPath = null;
                if (request.Photo != null)
                    photoPath = await _fileStorage.SavePhotoAsync(request.Photo);
                if (request.AadhaarCard != null)
                    aadhaarPath = await _fileStorage.SaveAadhaarAsync(request.AadhaarCard);

                await _staffService.UpdateAsync(userId, MapSaveDto(request, photoPath, aadhaarPath));

                if (request.RemovePhoto || photoPath != null)
                    _fileStorage.DeleteIfLocal(existing.Photo);
                if (request.RemoveAadhaarCard || aadhaarPath != null)
                    _fileStorage.DeleteIfLocal(existing.AadhaarCardUrl);

                return Ok(new ApiStatusResponse<StaffSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.Staff.Updated,
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
                return BadRequest(Fail(ApiMessages.Common.InvalidRequestBody));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Staff.UpdateFailed));
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
                    return NotFound(Fail(ApiMessages.Staff.NotFound));

                await _staffService.DeleteAsync(userId, _currentUser.AccountId);
                _fileStorage.DeleteIfLocal(existing.Photo);
                _fileStorage.DeleteIfLocal(existing.AadhaarCardUrl);

                return Ok(new ApiStatusResponse<StaffDeletedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.Staff.Deleted,
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
                return StatusCode(500, Fail(ApiMessages.Staff.DeleteFailed));
            }
        }

        private SaveStaffDto MapSaveDto(SaveStaffRequest request, string? photoPath, string? aadhaarPath)
        {
            return new SaveStaffDto
            {
                AccountId = _currentUser.AccountId,
                CreatedBy = _currentUser.UserId,
                FullName = request.FullName,
                Mobile = request.Mobile,
                Email = request.Email,
                Gender = request.Gender,
                AadhaarNumber = request.AadhaarNumber,
                EmployeeCode = request.EmployeeCode,
                JoiningDate = request.JoiningDate,
                Designation = request.Designation,
                Specialist = request.Specialist,
                BranchId = request.BranchId,
                SalaryRuleId = request.SalaryRuleId,
                Status = request.Status,
                AllowAppLogin = request.AllowAppLogin,
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

        private async Task<SaveStaffRequest> BindSaveRequestAsync()
        {
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
                    BranchId = ParseGuid(form["branch_id"]) ?? Guid.Empty,
                    SalaryRuleId = ParseGuid(form["salary_rule_id"]) ?? Guid.Empty,
                    Status = form["status"].ToString(),
                    AllowAppLogin = ParseBool(form["allow_app_login"]),
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
                throw new ArgumentException(ApiMessages.Common.RequestBodyRequired);

            return JsonSerializer.Deserialize<SaveStaffRequest>(json, JsonOptions)
                ?? throw new ArgumentException(ApiMessages.Common.InvalidRequestBody);
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
                ?? ApiMessages.Common.ValidationOccurred;
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
