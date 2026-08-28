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
    /// API Controller for Services management.
    /// Provides endpoints for CRUD operations on services.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public ServicesController(
            IServicesService servicesService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _servicesService = servicesService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new service.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ServicesResponse>> CreateServices([FromBody] CreateServicesRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateServicesDto>(request);
                var servicesDto = await _servicesService.CreateServicesAsync(createDto);
                var response = _mapper.Map<ServicesResponse>(servicesDto);
                return CreatedAtAction(nameof(GetServicesById), new { id = response.ServiceId }, response);
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
        /// Updates an existing service.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServicesResponse>> UpdateServices(Guid id, [FromBody] UpdateServicesRequest request)
        {
            try
            {
                var updateDto = _mapper.Map<UpdateServicesDto>(request);
                updateDto.ServiceId = id;
                var servicesDto = await _servicesService.UpdateServicesAsync(updateDto);
                var response = _mapper.Map<ServicesResponse>(servicesDto);
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
        /// Deletes a service by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServices(Guid id)
        {
            try
            {
                await _servicesService.DeleteServicesAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets a service by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicesResponse>> GetServicesById(Guid id)
        {
            var servicesDto = await _servicesService.GetServicesByIdAsync(id);
            if (servicesDto == null)
                return NotFound($"Service with ID {id} not found.");

            var response = _mapper.Map<ServicesResponse>(servicesDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all services for a specific account.
        /// </summary>
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<ServicesResponse>>> GetServicesByAccountId(Guid accountId)
        {
            var servicesDto = await _servicesService.GetServicesByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<ServicesResponse>>(servicesDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all services.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicesResponse>>> GetAllServices()
        {
            var servicesDto = await _servicesService.GetAllServicesAsync();
            var response = _mapper.Map<IEnumerable<ServicesResponse>>(servicesDto);
            return Ok(response);
        }
    }
}
#endif
