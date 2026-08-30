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
    /// Expense type list and management endpoints (API_044–API_048).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/expenses")]
    [Produces("application/json")]
    public class ExpensesTypesController : ControllerBase
    {
        private readonly IExpensesTypeService _expensesTypeService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public ExpensesTypesController(
            IExpensesTypeService expensesTypeService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            ICurrentUser currentUser)
        {
            _expensesTypeService = expensesTypeService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        /// <summary>API_044 Fetch Expense List</summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var list = await _expensesTypeService.GetListAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<ExpenseListDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.ExpenseListFetched,
                    Data = new ExpenseListDataResponse
                    {
                        Expenses = _mapper.Map<List<ExpenseListItemResponse>>(list)
                    }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.ExpenseListFailed));
            }
        }

        /// <summary>API_045 Fetch Expense Detail</summary>
        [HttpGet("{expenseId:guid}/details")]
        public async Task<IActionResult> GetDetails(Guid expenseId)
        {
            try
            {
                var detail = await _expensesTypeService.GetDetailsAsync(expenseId, _currentUser.AccountId);
                if (detail == null)
                    return NotFound(Fail(ApiMessages.ExpenseNotFound));

                return Ok(new ApiStatusResponse<ExpenseDetailResponse>
                {
                    Status = true,
                    Message = ApiMessages.ExpenseDetailsFetched,
                    Data = _mapper.Map<ExpenseDetailResponse>(detail)
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.ExpenseDetailsFailed));
            }
        }

        /// <summary>API_046 Create Expense</summary>
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] SaveExpenseRequest request)
        {
            try
            {
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                await _expensesTypeService.CreateAsync(MapSaveDto(request));
                return Ok(new ApiStatusResponse<ExpenseSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.ExpenseCreated,
                    Data = new ExpenseSavedDataResponse { Saved = true }
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
                return StatusCode(500, Fail(ApiMessages.ExpenseCreateFailed));
            }
        }

        /// <summary>API_047 Update Expense</summary>
        [HttpPost("{expenseId:guid}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Update(Guid expenseId, [FromBody] SaveExpenseRequest request)
        {
            try
            {
                var existing = await _expensesTypeService.GetDetailsAsync(expenseId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.ExpenseNotFound));

                if (IsStatusOnlyUpdate(request))
                {
                    await _expensesTypeService.UpdateStatusAsync(expenseId, _currentUser.AccountId, request.Status ?? string.Empty);
                    return Ok(new ApiStatusResponse<ExpenseSavedDataResponse>
                    {
                        Status = true,
                        Message = ApiMessages.ExpenseUpdated,
                        Data = new ExpenseSavedDataResponse { Saved = true }
                    });
                }

                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                await _expensesTypeService.UpdateAsync(expenseId, MapSaveDto(request));
                return Ok(new ApiStatusResponse<ExpenseSavedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.ExpenseUpdated,
                    Data = new ExpenseSavedDataResponse { Saved = true }
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
                return StatusCode(500, Fail(ApiMessages.ExpenseUpdateFailed));
            }
        }

        /// <summary>API_048 Delete Expense</summary>
        [HttpPost("{expenseId:guid}/delete")]
        public async Task<IActionResult> Delete(Guid expenseId)
        {
            try
            {
                var existing = await _expensesTypeService.GetDetailsAsync(expenseId, _currentUser.AccountId);
                if (existing == null)
                    return NotFound(Fail(ApiMessages.ExpenseNotFound));

                await _expensesTypeService.DeleteAsync(expenseId, _currentUser.AccountId);
                return Ok(new ApiStatusResponse<ExpenseDeletedDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.ExpenseDeleted,
                    Data = new ExpenseDeletedDataResponse { Deleted = true }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.ExpenseDeleteFailed));
            }
        }

        private SaveExpenseDto MapSaveDto(SaveExpenseRequest request)
        {
            return new SaveExpenseDto
            {
                AccountId = _currentUser.AccountId,
                CreatedBy = _currentUser.UserId,
                Name = request.Name ?? string.Empty,
                Description = request.Description,
                AllBranches = request.AllBranches ?? false,
                Branches = request.Branches,
                Status = request.Status ?? string.Empty
            };
        }

        private static bool IsStatusOnlyUpdate(SaveExpenseRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.Status)
                && string.IsNullOrWhiteSpace(request.Name)
                && request.AllBranches == null
                && (request.Branches == null || request.Branches.Count == 0);
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
