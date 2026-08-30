using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/management")]
    [Produces("application/json")]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly ICurrentUser _currentUser;

        public ManagementController(
            IManagementService managementService,
            IExceptionLogService exceptionLogService,
            ICurrentUser currentUser)
        {
            _managementService = managementService;
            _exceptionLogService = exceptionLogService;
            _currentUser = currentUser;
        }

        [HttpGet("account-summary")]
        public async Task<IActionResult> GetAccountSummary()
        {
            try
            {
                var result = await _managementService.GetAccountSummaryAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<AccountSummaryDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.AccountSummaryFetched,
                    Data = new AccountSummaryDataResponse
                    {
                        TotalBranches = result.TotalBranches,
                        TotalStaff = result.TotalStaff,
                        TotalServices = result.TotalServices,
                        TotalExpenses = result.TotalExpenses,
                        TotalSalaryRules = result.TotalSalaryRules
                    }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.AccountSummaryFailed));
            }
        }

        [HttpGet("feature-lock")]
        public async Task<IActionResult> GetFeatureLock()
        {
            try
            {
                var result = await _managementService.GetFeatureLockAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<FeatureLockDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.FeatureLockFetched,
                    Data = new FeatureLockDataResponse { FeatureLock = result.FeatureLock }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.FeatureLockFailed));
            }
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
