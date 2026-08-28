#if false // unused TBH APIs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// API Controller for TransactionRules management.
    /// Provides endpoints for CRUD operations on transaction rules.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionRulesController : ControllerBase
    {
        private readonly ITransactionRulesService _transactionRulesService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public TransactionRulesController(
            ITransactionRulesService transactionRulesService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _transactionRulesService = transactionRulesService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new transaction rule.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransactionRulesResponse>> CreateTransactionRules([FromBody] CreateTransactionRulesRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateTransactionRulesDto>(request);
                var transactionRulesDto = await _transactionRulesService.CreateTransactionRulesAsync(createDto);
                var response = _mapper.Map<TransactionRulesResponse>(transactionRulesDto);
                return CreatedAtAction(nameof(GetTransactionRulesById), new { id = response.TransactionRuleId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred" });
            }
        }

        /// <summary>
        /// Updates an existing transaction rule.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionRulesResponse>> UpdateTransactionRules(Guid id, [FromBody] UpdateTransactionRulesRequest request)
        {
            try
            {
                var updateDto = _mapper.Map<UpdateTransactionRulesDto>(request);
                updateDto.TransactionRuleId = id;
                var transactionRulesDto = await _transactionRulesService.UpdateTransactionRulesAsync(updateDto);
                var response = _mapper.Map<TransactionRulesResponse>(transactionRulesDto);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred" });
            }
        }

        /// <summary>
        /// Deletes a transaction rule by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransactionRules(Guid id)
        {
            try
            {
                await _transactionRulesService.DeleteTransactionRulesAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets a transaction rule by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionRulesResponse>> GetTransactionRulesById(Guid id)
        {
            var transactionRulesDto = await _transactionRulesService.GetTransactionRulesByIdAsync(id);
            if (transactionRulesDto == null)
                return NotFound($"Transaction rule with ID {id} not found.");

            var response = _mapper.Map<TransactionRulesResponse>(transactionRulesDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all transaction rules for a specific account.
        /// </summary>
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionRulesResponse>>> GetTransactionRulesByAccountId(Guid accountId)
        {
            var transactionRulesDto = await _transactionRulesService.GetTransactionRulesByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<TransactionRulesResponse>>(transactionRulesDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all transaction rules.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionRulesResponse>>> GetAllTransactionRules()
        {
            var transactionRulesDto = await _transactionRulesService.GetAllTransactionRulesAsync();
            var response = _mapper.Map<IEnumerable<TransactionRulesResponse>>(transactionRulesDto);
            return Ok(response);
        }
    }
}
#endif
