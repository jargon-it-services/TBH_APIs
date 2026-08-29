using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// Salary rule catalog and management endpoints (API_038–API_043).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/salary-rules")]
    [Produces("application/json")]
    public class SalaryRulesController : ControllerBase
    {
        private readonly ISalaryRuleService _salaryRuleService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public SalaryRulesController(
            ISalaryRuleService salaryRuleService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            ICurrentUser currentUser)
        {
            _salaryRuleService = salaryRuleService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        /// <summary>API_038 Fetch Salary Rules Catalog</summary>
        [HttpGet]
        public async Task<IActionResult> GetCatalog()
        {
            try
            {
                var catalog = await _salaryRuleService.GetCatalogAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<SalaryRuleCatalogDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.SalaryRuleCatalogFetched,
                    Data = new SalaryRuleCatalogDataResponse
                    {
                        SalaryRules = _mapper.Map<List<SalaryRuleCatalogItemResponse>>(catalog)
                    }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.SalaryRuleCatalogFailed));
            }
        }

        /// <summary>API_039 Fetch Salary Rule List</summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var list = await _salaryRuleService.GetListAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<SalaryRuleListDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.SalaryRuleListFetched,
                    Data = new SalaryRuleListDataResponse
                    {
                        SalaryRules = _mapper.Map<List<SalaryRuleListItemResponse>>(list)
                    }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.SalaryRuleListFailed));
            }
        }

        /// <summary>API_040 Fetch Salary Rule Detail</summary>
        [HttpGet("{ruleId:guid}/details")]
        public async Task<IActionResult> GetDetails(Guid ruleId)
        {
            try
            {
                var detail = await _salaryRuleService.GetDetailsAsync(ruleId, _currentUser.AccountId);
                if (detail == null)
                    return NotFound(Fail(ApiMessages.SalaryRuleNotFound));

                return Ok(new ApiStatusResponse<SalaryRuleDetailResponse>
                {
                    Status = true,
                    Message = ApiMessages.SalaryRuleDetailsFetched,
                    Data = _mapper.Map<SalaryRuleDetailResponse>(detail)
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.SalaryRuleDetailsFailed));
            }
        }

        /// <summary>API_041 Create Salary Rule</summary>
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] SaveSalaryRuleRequest request)
        {
            try
            {
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                await _salaryRuleService.CreateAsync(MapSaveDto(request));
                return Ok(new ApiStatusResponse<SalaryRuleSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.SalaryRuleCreated,
                    Data = new SalaryRuleSavedDataResponse { Saved = true }
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
                return StatusCode(500, Fail(ApiMessages.SalaryRuleCreateFailed));
            }
        }

        /// <summary>API_042 Update Salary Rule</summary>
        [HttpPost("{ruleId:guid}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Update(Guid ruleId, [FromBody] SaveSalaryRuleRequest request)
        {
            try
            {
                var existing = await _salaryRuleService.GetDetailsAsync(ruleId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.SalaryRuleNotFound));

                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                await _salaryRuleService.UpdateAsync(ruleId, MapSaveDto(request));
                return Ok(new ApiStatusResponse<SalaryRuleSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.SalaryRuleUpdated,
                    Data = new SalaryRuleSavedDataResponse { Saved = true }
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
                return StatusCode(500, Fail(ApiMessages.SalaryRuleUpdateFailed));
            }
        }

        /// <summary>API_043 Delete Salary Rule</summary>
        [HttpPost("{ruleId:guid}/delete")]
        public async Task<IActionResult> Delete(Guid ruleId)
        {
            try
            {
                var existing = await _salaryRuleService.GetDetailsAsync(ruleId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.SalaryRuleNotFound));

                await _salaryRuleService.DeleteAsync(ruleId, _currentUser.AccountId);
                return Ok(new ApiStatusResponse<SalaryRuleDeletedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.SalaryRuleDeleted,
                    Data = new SalaryRuleDeletedDataResponse { Deleted = true }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.SalaryRuleDeleteFailed));
            }
        }

        private SaveSalaryRuleDto MapSaveDto(SaveSalaryRuleRequest request)
        {
            return new SaveSalaryRuleDto
            {
                AccountId = _currentUser.AccountId,
                Name = request.Name,
                Description = request.Description,
                SalaryType = request.SalaryType,
                FixedSalary = request.FixedSalary,
                MonthlyTarget = request.MonthlyTarget,
                TargetBonus = request.TargetBonus,
                AllowAdvanceRecovery = request.AllowAdvanceRecovery ?? false,
                MaxRecoveryPerMonth = request.MaxRecoveryPerMonth,
                Status = request.Status
            };
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
    }
}
