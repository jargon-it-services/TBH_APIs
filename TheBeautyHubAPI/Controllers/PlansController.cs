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
    public class PlansController : ControllerBase
    {
        private readonly IPlansService _plansService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public PlansController(
            IPlansService plansService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _plansService = plansService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all plans
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlanResponse>), 200)]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _plansService.GetAllPlansAsync();
            var response = _mapper.Map<IEnumerable<PlanResponse>>(plans);
            return Ok(response);
        }

        /// <summary>
        /// Get active plans only
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<PlanResponse>), 200)]
        public async Task<IActionResult> GetActivePlans()
        {
            var plans = await _plansService.GetActivePlansAsync();
            var response = _mapper.Map<IEnumerable<PlanResponse>>(plans);
            return Ok(response);
        }

        /// <summary>
        /// Get plan by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PlanResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPlanById(Guid id)
        {
            var plan = await _plansService.GetPlanByIdAsync(id);
            if (plan == null)
                return NotFound(new { message = "Plan not found." });

            var response = _mapper.Map<PlanResponse>(plan);
            return Ok(response);
        }

        /// <summary>
        /// Create a new plan
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PlanResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createDto = _mapper.Map<CreatePlanDto>(request);
                var plan = await _plansService.CreatePlanAsync(createDto);
                var response = _mapper.Map<PlanResponse>(plan);

                return CreatedAtAction(nameof(GetPlanById), new { id = response.PlanId }, response);
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
        /// Update an existing plan
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PlanResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] UpdatePlanRequest request)
        {
            if (id != request.PlanId)
                return BadRequest(new { message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updateDto = _mapper.Map<UpdatePlanDto>(request);
                var plan = await _plansService.UpdatePlanAsync(updateDto);
                var response = _mapper.Map<PlanResponse>(plan);

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
        /// Delete a plan
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePlan(Guid id)
        {
            try
            {
                await _plansService.DeletePlanAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
#endif
