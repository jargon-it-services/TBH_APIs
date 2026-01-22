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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public SubscriptionsController(
            ISubscriptionService subscriptionService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _subscriptionService = subscriptionService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all subscriptions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionResponse>), 200)]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            var response = _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            return Ok(response);
        }

        /// <summary>
        /// Get subscription by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SubscriptionResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubscriptionById(Guid id)
        {
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
                return NotFound(new { message = "Subscription not found." });

            var response = _mapper.Map<SubscriptionResponse>(subscription);
            return Ok(response);
        }

        /// <summary>
        /// Get subscriptions by Account ID
        /// </summary>
        [HttpGet("account/{accountId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionResponse>), 200)]
        public async Task<IActionResult> GetSubscriptionsByAccountId(Guid accountId)
        {
            var subscriptions = await _subscriptionService.GetSubscriptionsByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            return Ok(response);
        }

        /// <summary>
        /// Get active subscriptions by Account ID
        /// </summary>
        [HttpGet("account/{accountId:guid}/active")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionResponse>), 200)]
        public async Task<IActionResult> GetActiveSubscriptionsByAccountId(Guid accountId)
        {
            var subscriptions = await _subscriptionService.GetActiveSubscriptionsByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            return Ok(response);
        }

        /// <summary>
        /// Get subscriptions by Plan ID
        /// </summary>
        [HttpGet("plan/{planId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionResponse>), 200)]
        public async Task<IActionResult> GetSubscriptionsByPlanId(Guid planId)
        {
            var subscriptions = await _subscriptionService.GetSubscriptionsByPlanIdAsync(planId);
            var response = _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            return Ok(response);
        }

        /// <summary>
        /// Create a new subscription
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SubscriptionResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createDto = _mapper.Map<CreateSubscriptionDto>(request);
                var subscription = await _subscriptionService.CreateSubscriptionAsync(createDto);
                var response = _mapper.Map<SubscriptionResponse>(subscription);

                return CreatedAtAction(nameof(GetSubscriptionById), new { id = response.SubscriptionId }, response);
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
        /// Update an existing subscription
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SubscriptionResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateSubscription(Guid id, [FromBody] UpdateSubscriptionRequest request)
        {
            if (id != request.SubscriptionId)
                return BadRequest(new { message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updateDto = _mapper.Map<UpdateSubscriptionDto>(request);
                var subscription = await _subscriptionService.UpdateSubscriptionAsync(updateDto);
                var response = _mapper.Map<SubscriptionResponse>(subscription);

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
        /// Delete a subscription
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubscription(Guid id)
        {
            try
            {
                await _subscriptionService.DeleteSubscriptionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

