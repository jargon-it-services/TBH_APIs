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
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FirmsController : ControllerBase
    {
        private readonly IFirmService _firmService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public FirmsController(
            IFirmService firmService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _firmService = firmService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all firms
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FirmResponse>), 200)]
        public async Task<IActionResult> GetAllFirms()
        {
            var firms = await _firmService.GetAllFirmsAsync();
            var response = _mapper.Map<IEnumerable<FirmResponse>>(firms);
            return Ok(response);
        }

        /// <summary>
        /// Get firm by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FirmResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFirmById(Guid id)
        {
            var firm = await _firmService.GetFirmByIdAsync(id);
            if (firm == null)
                return NotFound(new { message = "Firm not found." });

            var response = _mapper.Map<FirmResponse>(firm);
            return Ok(response);
        }

        /// <summary>
        /// Get firms by Account ID
        /// </summary>
        [HttpGet("account/{accountId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<FirmResponse>), 200)]
        public async Task<IActionResult> GetFirmsByAccountId(Guid accountId)
        {
            var firms = await _firmService.GetFirmsByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<FirmResponse>>(firms);
            return Ok(response);
        }

        /// <summary>
        /// Create a new firm
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FirmResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFirm([FromBody] CreateFirmRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createDto = _mapper.Map<CreateFirmDto>(request);
                var firm = await _firmService.CreateFirmAsync(createDto);
                var response = _mapper.Map<FirmResponse>(firm);

                return CreatedAtAction(nameof(GetFirmById), new { id = response.FirmId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while creating the firm" });
            }
        }

        /// <summary>
        /// Update an existing firm
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(FirmResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFirm(Guid id, [FromBody] UpdateFirmRequest request)
        {
            if (id != request.FirmId)
                return BadRequest(new { message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updateDto = _mapper.Map<UpdateFirmDto>(request);
                var firm = await _firmService.UpdateFirmAsync(updateDto);
                var response = _mapper.Map<FirmResponse>(firm);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while updating the firm" });
            }
        }

        /// <summary>
        /// Delete a firm
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteFirm(Guid id)
        {
            try
            {
                await _firmService.DeleteFirmAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while deleting the firm" });
            }
        }
    }
}
#endif
