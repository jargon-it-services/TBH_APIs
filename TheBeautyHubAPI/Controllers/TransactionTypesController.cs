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
    /// API Controller for TransactionType management.
    /// Provides endpoints for CRUD operations on transaction types.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypesController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public TransactionTypesController(
            ITransactionTypeService transactionTypeService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _transactionTypeService = transactionTypeService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new transaction type.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransactionTypeResponse>> CreateTransactionType([FromBody] CreateTransactionTypeRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateTransactionTypeDto>(request);
                var transactionTypeDto = await _transactionTypeService.CreateTransactionTypeAsync(createDto);
                var response = _mapper.Map<TransactionTypeResponse>(transactionTypeDto);
                return CreatedAtAction(nameof(GetTransactionTypeById), new { id = response.TransactionTypeId }, response);
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
        /// Updates an existing transaction type.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionTypeResponse>> UpdateTransactionType(Guid id, [FromBody] UpdateTransactionTypeRequest request)
        {
            try
            {
                var updateDto = _mapper.Map<UpdateTransactionTypeDto>(request);
                updateDto.TransactionTypeId = id;
                var transactionTypeDto = await _transactionTypeService.UpdateTransactionTypeAsync(updateDto);
                var response = _mapper.Map<TransactionTypeResponse>(transactionTypeDto);
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
        /// Deletes a transaction type by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransactionType(Guid id)
        {
            try
            {
                await _transactionTypeService.DeleteTransactionTypeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets a transaction type by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionTypeResponse>> GetTransactionTypeById(Guid id)
        {
            var transactionTypeDto = await _transactionTypeService.GetTransactionTypeByIdAsync(id);
            if (transactionTypeDto == null)
                return NotFound($"Transaction type with ID {id} not found.");

            var response = _mapper.Map<TransactionTypeResponse>(transactionTypeDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all transaction types.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionTypeResponse>>> GetAllTransactionTypes()
        {
            var transactionTypesDto = await _transactionTypeService.GetAllTransactionTypesAsync();
            var response = _mapper.Map<IEnumerable<TransactionTypeResponse>>(transactionTypesDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all active transaction types.
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TransactionTypeResponse>>> GetActiveTransactionTypes()
        {
            var transactionTypesDto = await _transactionTypeService.GetActiveTransactionTypesAsync();
            var response = _mapper.Map<IEnumerable<TransactionTypeResponse>>(transactionTypesDto);
            return Ok(response);
        }
    }
}

