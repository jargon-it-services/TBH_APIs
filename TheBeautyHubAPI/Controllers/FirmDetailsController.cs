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
    public class FirmDetailsController : ControllerBase
    {
        private readonly IFirmDetailsService _firmDetailsService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public FirmDetailsController(
            IFirmDetailsService firmDetailsService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _firmDetailsService = firmDetailsService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all firm details
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FirmDetailsResponse>), 200)]
        public async Task<IActionResult> GetAllFirmDetails()
        {
            var firmDetails = await _firmDetailsService.GetAllFirmDetailsAsync();
            var response = _mapper.Map<IEnumerable<FirmDetailsResponse>>(firmDetails);
            return Ok(response);
        }

        /// <summary>
        /// Get firm details by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FirmDetailsResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFirmDetailsById(Guid id)
        {
            var firmDetails = await _firmDetailsService.GetFirmDetailsByIdAsync(id);
            if (firmDetails == null)
                return NotFound(new { message = "Firm details not found." });

            var response = _mapper.Map<FirmDetailsResponse>(firmDetails);
            return Ok(response);
        }

        /// <summary>
        /// Get firm details by Firm ID
        /// </summary>
        [HttpGet("firm/{firmId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<FirmDetailsResponse>), 200)]
        public async Task<IActionResult> GetFirmDetailsByFirmId(Guid firmId)
        {
            var firmDetails = await _firmDetailsService.GetFirmDetailsByFirmIdAsync(firmId);
            var response = _mapper.Map<IEnumerable<FirmDetailsResponse>>(firmDetails);
            return Ok(response);
        }

        /// <summary>
        /// Get firm details by User ID
        /// </summary>
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<FirmDetailsResponse>), 200)]
        public async Task<IActionResult> GetFirmDetailsByUserId(Guid userId)
        {
            var firmDetails = await _firmDetailsService.GetFirmDetailsByUserIdAsync(userId);
            var response = _mapper.Map<IEnumerable<FirmDetailsResponse>>(firmDetails);
            return Ok(response);
        }

        /// <summary>
        /// Get firm details by Account ID
        /// </summary>
        [HttpGet("account/{accountId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<FirmDetailsResponse>), 200)]
        public async Task<IActionResult> GetFirmDetailsByAccountId(Guid accountId)
        {
            var firmDetails = await _firmDetailsService.GetFirmDetailsByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<FirmDetailsResponse>>(firmDetails);
            return Ok(response);
        }

        /// <summary>
        /// Create new firm details
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FirmDetailsResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFirmDetails([FromBody] CreateFirmDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createDto = _mapper.Map<CreateFirmDetailsDto>(request);
                var firmDetails = await _firmDetailsService.CreateFirmDetailsAsync(createDto);
                var response = _mapper.Map<FirmDetailsResponse>(firmDetails);

                return CreatedAtAction(nameof(GetFirmDetailsById), new { id = response.FirmDetailsId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred" });
            }
        }

        /// <summary>
        /// Update existing firm details
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(FirmDetailsResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFirmDetails(Guid id, [FromBody] UpdateFirmDetailsRequest request)
        {
            if (id != request.FirmDetailsId)
                return BadRequest(new { message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updateDto = _mapper.Map<UpdateFirmDetailsDto>(request);
                var firmDetails = await _firmDetailsService.UpdateFirmDetailsAsync(updateDto);
                var response = _mapper.Map<FirmDetailsResponse>(firmDetails);

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
                return StatusCode(500, new { error = "An error occurred" });
            }
        }

        /// <summary>
        /// Delete firm details
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteFirmDetails(Guid id)
        {
            try
            {
                await _firmDetailsService.DeleteFirmDetailsAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

