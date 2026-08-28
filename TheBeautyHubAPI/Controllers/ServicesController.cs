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
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// Service catalog and management endpoints (API_025–API_030).
    /// Requires an AuthCenter Bearer access token.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/services")]
    [Produces("application/json")]
    public class ServicesController : ControllerBase
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IServicesService _servicesService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly ServicePhotoStorage _photoStorage;
        private readonly ICurrentUser _currentUser;

        public ServicesController(
            IServicesService servicesService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            ServicePhotoStorage photoStorage,
            ICurrentUser currentUser)
        {
            _servicesService = servicesService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _photoStorage = photoStorage;
            _currentUser = currentUser;
        }

        /// <summary>API_025 Fetch Services Catalog</summary>
        [HttpGet]
        public async Task<IActionResult> GetCatalog()
        {
            try
            {
                var catalog = await _servicesService.GetCatalogAsync(_currentUser.AccountId);
                var items = _mapper.Map<List<ServiceCatalogItemResponse>>(catalog);
                return Ok(new ApiStatusResponse<ServiceCatalogDataResponse>
                {
                    Status = true,
                    Data = new ServiceCatalogDataResponse { Services = items }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail("An error occurred while fetching the services catalog."));
            }
        }

        /// <summary>API_026 Fetch Service List</summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var list = await _servicesService.GetListAsync(_currentUser.AccountId);
                var items = _mapper.Map<List<ServiceListItemResponse>>(list);
                foreach (var item in items)
                    item.Photo = ToAbsoluteUrl(item.Photo);

                return Ok(new ApiStatusResponse<ServiceListDataResponse>
                {
                    Status = true,
                    Data = new ServiceListDataResponse { Services = items }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail("An error occurred while fetching services."));
            }
        }

        /// <summary>API_027 Fetch Service Detail</summary>
        [HttpGet("{serviceId:guid}/details")]
        public async Task<IActionResult> GetDetails(Guid serviceId)
        {
            try
            {
                var detail = await _servicesService.GetDetailsAsync(serviceId, _currentUser.AccountId);
                if (detail == null)
                    return NotFound(Fail("Service not found."));

                var response = _mapper.Map<ServiceDetailResponse>(detail);
                response.Photo = ToAbsoluteUrl(response.Photo);

                return Ok(new ApiStatusResponse<ServiceDetailResponse>
                {
                    Status = true,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail("An error occurred while fetching service details."));
            }
        }

        /// <summary>API_028 Create Service</summary>
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                var request = await BindSaveRequestAsync();
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? savedPhoto = null;
                if (request.Photo != null)
                    savedPhoto = await _photoStorage.SaveAsync(request.Photo);

                var dto = MapSaveDto(request, savedPhoto, hasNewPhoto: savedPhoto != null);
                await _servicesService.CreateAsync(dto);

                return Ok(new ApiStatusResponse<ServiceSavedDataResponse>
                {
                    Status = true,
                    Data = new ServiceSavedDataResponse { Saved = true }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Fail(ex.Message));
            }
            catch (JsonException ex)
            {
                return BadRequest(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail("An error occurred while creating the service."));
            }
        }

        /// <summary>API_029 Update Service</summary>
        [HttpPost("{serviceId:guid}")]
        public async Task<IActionResult> Update(Guid serviceId)
        {
            try
            {
                var existing = await _servicesService.GetDetailsAsync(serviceId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail("Service not found."));

                var request = await BindSaveRequestAsync();
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                string? savedPhoto = null;
                if (request.Photo != null)
                    savedPhoto = await _photoStorage.SaveAsync(request.Photo);

                var dto = MapSaveDto(request, savedPhoto, hasNewPhoto: savedPhoto != null);
                await _servicesService.UpdateAsync(serviceId, dto);

                if (request.RemovePhoto || savedPhoto != null)
                    _photoStorage.DeleteIfLocal(existing.Photo);

                return Ok(new ApiStatusResponse<ServiceSavedDataResponse>
                {
                    Status = true,
                    Data = new ServiceSavedDataResponse { Saved = true }
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
            catch (JsonException ex)
            {
                return BadRequest(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail("An error occurred while updating the service."));
            }
        }

        /// <summary>API_030 Delete Service</summary>
        [HttpPost("{serviceId:guid}/delete")]
        public async Task<IActionResult> Delete(Guid serviceId)
        {
            try
            {
                var existing = await _servicesService.GetDetailsAsync(serviceId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail("Service not found."));

                await _servicesService.DeleteAsync(serviceId, _currentUser.AccountId);
                _photoStorage.DeleteIfLocal(existing.Photo);

                return Ok(new ApiStatusResponse<ServiceDeletedDataResponse>
                {
                    Status = true,
                    Data = new ServiceDeletedDataResponse { Deleted = true }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail("An error occurred while deleting the service."));
            }
        }

        private SaveServiceDto MapSaveDto(SaveServiceRequest request, string? photoPath, bool hasNewPhoto)
        {
            return new SaveServiceDto
            {
                AccountId = _currentUser.AccountId,
                CreatedBy = _currentUser.UserId,
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                DurationMinutes = request.DurationMinutes ?? 0,
                ApplicableGender = request.ApplicableGender,
                Type = request.Type,
                Status = request.Status,
                CustomerPrice = request.CustomerPrice ?? 0,
                MaterialCost = request.MaterialCost ?? 0,
                CommissionType = request.CommissionType,
                CommissionValue = request.CommissionValue ?? 0,
                OtherCost = request.OtherCost ?? 0,
                HomeServiceAvailable = request.HomeServiceAvailable ?? false,
                HomeVisitCharges = request.HomeVisitCharges,
                ServiceRadiusKm = request.ServiceRadiusKm,
                ExtraChargePerKm = request.ExtraChargePerKm,
                AllBranches = request.AllBranches ?? false,
                Branches = request.Branches,
                Photo = photoPath,
                RemovePhoto = request.RemovePhoto,
                HasNewPhoto = hasNewPhoto
            };
        }

        private async Task<SaveServiceRequest> BindSaveRequestAsync()
        {
            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync();
                return new SaveServiceRequest
                {
                    Name = form["name"].ToString(),
                    Description = form["description"].ToString(),
                    Category = form["category"].ToString(),
                    DurationMinutes = FormHas(form, "duration_minutes") ? ParseInt(form["duration_minutes"]) : null,
                    ApplicableGender = form["applicable_gender"].ToString(),
                    Type = form["type"].ToString(),
                    Status = form["status"].ToString(),
                    CustomerPrice = ParseDecimal(form["customer_price"]),
                    MaterialCost = ParseDecimal(form["material_cost"]),
                    CommissionType = form["commission_type"].ToString(),
                    CommissionValue = ParseDecimal(form["commission_value"]),
                    OtherCost = ParseDecimal(form["other_cost"]),
                    HomeServiceAvailable = FormHas(form, "home_service_available") ? ParseBool(form["home_service_available"]) : null,
                    HomeVisitCharges = ParseDecimal(form["home_visit_charges"]),
                    ServiceRadiusKm = ParseDecimal(form["service_radius_km"]),
                    ExtraChargePerKm = ParseDecimal(form["extra_charge_per_km"]),
                    AllBranches = FormHas(form, "all_branches") ? ParseBool(form["all_branches"]) : null,
                    Branches = FormHas(form, "branches") ? ParseGuidList(form["branches"]) : null,
                    RemovePhoto = ParseBool(form["remove_photo"]),
                    Photo = form.Files.GetFile("photo")
                };
            }

            using var reader = new StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Request body is required.");

            var request = JsonSerializer.Deserialize<SaveServiceRequest>(json, JsonOptions);
            if (request == null)
                throw new ArgumentException("Invalid request body.");

            return request;
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
                ?? "Invalid request.";
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

        private static bool FormHas(IFormCollection form, string key)
        {
            return form.ContainsKey(key);
        }

        private static int? ParseInt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : null;
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

        private static List<Guid> ParseGuidList(Microsoft.Extensions.Primitives.StringValues values)
        {
            var ids = new List<Guid>();
            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                var trimmed = value.Trim();
                if (trimmed.StartsWith('['))
                {
                    var parsed = JsonSerializer.Deserialize<List<Guid>>(trimmed, JsonOptions);
                    if (parsed != null)
                        ids.AddRange(parsed);
                    continue;
                }

                foreach (var part in trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (Guid.TryParse(part, out var id))
                        ids.Add(id);
                }
            }

            return ids.Distinct().ToList();
        }
    }
}
