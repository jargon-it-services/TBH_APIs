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
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// Transaction bootstrap, create, update, list, and detail endpoints (API_049–API_054).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/transactions")]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly ITransactionService _transactionService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;

        public TransactionsController(
            ITransactionService transactionService,
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            ICurrentUser currentUser,
            IUserService userService)
        {
            _transactionService = transactionService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _currentUser = currentUser;
            _userService = userService;
        }

        /// <summary>API_049 Fetch Transaction Bootstrap</summary>
        [HttpGet("bootstrap")]
        public async Task<IActionResult> GetBootstrap()
        {
            try
            {
                var data = await _transactionService.GetBootstrapAsync(
                    _currentUser.AccountId,
                    _currentUser.UserId,
                    _currentUser.Roles);
                return Ok(new ApiStatusResponse<TransactionBootstrapResponse>
                {
                    Status = true,
                    Message = ApiMessages.Transaction.BootstrapFetched,
                    Data = _mapper.Map<TransactionBootstrapResponse>(data)
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Transaction.BootstrapFailed));
            }
        }

        /// <summary>API_053 Fetch Transactions List</summary>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var list = await _transactionService.GetListAsync(_currentUser.AccountId);
                return Ok(new ApiStatusResponse<TransactionListDataResponse>
                {
                    Status = true,
                    Message = ApiMessages.Transaction.ListFetched,
                    Data = new TransactionListDataResponse
                    {
                        Meta = new TransactionListMetaResponse { FeatureLock = list.FeatureLock },
                        Filters = _mapper.Map<TransactionListFiltersResponse>(list.Filters),
                        Transactions = _mapper.Map<List<TransactionListItemResponse>>(list.Transactions)
                    }
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Transaction.ListFailed));
            }
        }

        /// <summary>API_054 Fetch Transaction Details</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            try
            {
                var detail = await _transactionService.GetDetailsAsync(id, _currentUser.AccountId);
                if (detail == null)
                    return NotFound(Fail(ApiMessages.Transaction.NotFound));

                return Ok(new ApiStatusResponse<TransactionRecordResponse>
                {
                    Status = true,
                    Message = ApiMessages.Transaction.DetailsFetched,
                    Data = _mapper.Map<TransactionRecordResponse>(detail)
                });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Transaction.DetailsFailed));
            }
        }

        /// <summary>API_050 Create Transaction</summary>
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                var request = await BindSaveRequestAsync(requireIdempotency: true);
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                var saved = await _transactionService.CreateAsync(await MapSaveDto(request));
                return Ok(new ApiStatusResponse<TransactionSavedResponse>
                {
                    Status = true,
                    Message = ApiMessages.Transaction.Created,
                    Data = _mapper.Map<TransactionSavedResponse>(saved)
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
                return StatusCode(500, Fail(ApiMessages.Transaction.CreateFailed));
            }
        }

        /// <summary>API_051 Update Transaction</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var request = await BindSaveRequestAsync(requireIdempotency: false);
                TryValidateModel(request);
                if (!ModelState.IsValid)
                    return BadRequest(Fail(GetModelStateError()));

                var saved = await _transactionService.UpdateAsync(id, await MapSaveDto(request));
                return Ok(new ApiStatusResponse<TransactionSavedResponse>
                {
                    Status = true,
                    Message = ApiMessages.Transaction.Updated,
                    Data = _mapper.Map<TransactionSavedResponse>(saved)
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (InvalidOperationException ex) when (ex.Message == TransactionService.EditWindowClosedCode)
            {
                return Conflict(Fail(ApiMessages.Transaction.EditWindowClosed));
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
                return StatusCode(500, Fail(ApiMessages.Transaction.UpdateFailed));
            }
        }

        /// <summary>API_052 Mark Transaction Paid</summary>
        [HttpPost("{id}/mark-paid")]
        public async Task<IActionResult> MarkPaid(string id)
        {
            try
            {
                var saved = await _transactionService.MarkPaidAsync(id, _currentUser.AccountId);
                return Ok(new ApiStatusResponse<TransactionMarkPaidResponse>
                {
                    Status = true,
                    Message = ApiMessages.Transaction.MarkedPaid,
                    Data = new TransactionMarkPaidResponse
                    {
                        Id = saved.Id,
                        Status = saved.Status,
                        PaidAt = saved.PaidAt
                    }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Fail(ex.Message));
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex, _currentUser.UserId);
                return StatusCode(500, Fail(ApiMessages.Transaction.MarkPaidFailed));
            }
        }

        private async Task<SaveTransactionDto> MapSaveDto(SaveTransactionRequest request)
        {
            var editorName = await ResolveEditorNameAsync();
            return new SaveTransactionDto
            {
                AccountId = _currentUser.AccountId,
                UserId = _currentUser.UserId,
                EditorName = editorName,
                IdempotencyKey = request.IdempotencyKey,
                Type = request.Type,
                BranchId = request.BranchId ?? Guid.Empty,
                PaymentMode = request.PaymentMode,
                Services = (request.Services ?? new List<SaveTransactionLineRequest>())
                    .Select(s => new SaveTransactionLineDto
                    {
                        ServiceId = s.ServiceId,
                        Quantity = s.Quantity,
                        StaffId = s.StaffId
                    })
                    .ToList(),
                CustomerName = request.CustomerName,
                CustomerMobile = request.CustomerMobile,
                Remark = request.Remark,
                StaffId = request.StaffId,
                CouponCode = request.CouponCode
            };
        }

        private async Task<string?> ResolveEditorNameAsync()
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(_currentUser.UserId);
                return user?.UserName;
            }
            catch
            {
                return null;
            }
        }

        private async Task<SaveTransactionRequest> BindSaveRequestAsync(bool requireIdempotency)
        {
            using var reader = new StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException(ApiMessages.Common.RequestBodyRequired);

            var request = JsonSerializer.Deserialize<SaveTransactionRequest>(json, JsonOptions)
                ?? throw new ArgumentException(ApiMessages.Common.InvalidRequestBody);

            if (requireIdempotency && string.IsNullOrWhiteSpace(request.IdempotencyKey))
                throw new ArgumentException(ApiMessages.Transaction.IdempotencyRequired);

            return request;
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
    }
}
