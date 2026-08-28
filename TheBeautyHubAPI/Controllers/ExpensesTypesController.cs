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
    /// API Controller for ExpensesType management.
    /// Provides endpoints for CRUD operations on expenses types.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesTypesController : ControllerBase
    {
        private readonly IExpensesTypeService _expensesTypeService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public ExpensesTypesController(
            IExpensesTypeService expensesTypeService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _expensesTypeService = expensesTypeService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new expenses type.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ExpensesTypeResponse>> CreateExpensesType([FromBody] CreateExpensesTypeRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateExpensesTypeDto>(request);
                var expensesTypeDto = await _expensesTypeService.CreateExpensesTypeAsync(createDto);
                var response = _mapper.Map<ExpensesTypeResponse>(expensesTypeDto);
                return CreatedAtAction(nameof(GetExpensesTypeById), new { id = response.ExpensesTypeId }, response);
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
        /// Updates an existing expenses type.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ExpensesTypeResponse>> UpdateExpensesType(Guid id, [FromBody] UpdateExpensesTypeRequest request)
        {
            try
            {
                var updateDto = _mapper.Map<UpdateExpensesTypeDto>(request);
                updateDto.ExpensesTypeId = id;
                var expensesTypeDto = await _expensesTypeService.UpdateExpensesTypeAsync(updateDto);
                var response = _mapper.Map<ExpensesTypeResponse>(expensesTypeDto);
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
        /// Deletes an expenses type by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpensesType(Guid id)
        {
            try
            {
                await _expensesTypeService.DeleteExpensesTypeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets an expenses type by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpensesTypeResponse>> GetExpensesTypeById(Guid id)
        {
            var expensesTypeDto = await _expensesTypeService.GetExpensesTypeByIdAsync(id);
            if (expensesTypeDto == null)
                return NotFound($"Expenses type with ID {id} not found.");

            var response = _mapper.Map<ExpensesTypeResponse>(expensesTypeDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all expenses types for a specific account.
        /// </summary>
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<ExpensesTypeResponse>>> GetExpensesTypesByAccountId(Guid accountId)
        {
            var expensesTypesDto = await _expensesTypeService.GetExpensesTypesByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<ExpensesTypeResponse>>(expensesTypesDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all expenses types.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpensesTypeResponse>>> GetAllExpensesTypes()
        {
            var expensesTypesDto = await _expensesTypeService.GetAllExpensesTypesAsync();
            var response = _mapper.Map<IEnumerable<ExpensesTypeResponse>>(expensesTypesDto);
            return Ok(response);
        }
    }
}
#endif
