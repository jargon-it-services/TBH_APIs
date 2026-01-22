using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSessionsController : ControllerBase
    {
        private readonly IUserSessionService _service;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public UserSessionsController(
            IUserSessionService service,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _service = service;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserSessionResponse>> Create([FromBody] CreateUserSessionRequest request)
        {
            try
            {
                var dto = _mapper.Map<CreateUserSessionDto>(request);
                var result = await _service.CreateAsync(dto);
                var response = _mapper.Map<UserSessionResponse>(result);
                return CreatedAtAction(nameof(GetById), new { id = response.SessionId }, response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserSessionResponse>> Update(Guid id, [FromBody] UpdateUserSessionRequest request)
        {
            try
            {
                var dto = _mapper.Map<UpdateUserSessionDto>(request);
                var result = await _service.UpdateAsync(id, dto);
                var response = _mapper.Map<UserSessionResponse>(result);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/revoke")]
        public async Task<ActionResult> Revoke(Guid id, [FromBody] string? revocationReason = null)
        {
            try
            {
                var result = await _service.RevokeAsync(id, revocationReason);
                if (result)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (result)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserSessionResponse>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<UserSessionResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserSessionResponse>>> GetByUserId(Guid userId)
        {
            try
            {
                var result = await _service.GetByUserIdAsync(userId);
                var response = _mapper.Map<IEnumerable<UserSessionResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}/active")]
        public async Task<ActionResult<IEnumerable<UserSessionResponse>>> GetActiveSessions(Guid userId)
        {
            try
            {
                var result = await _service.GetActiveSessionsAsync(userId);
                var response = _mapper.Map<IEnumerable<UserSessionResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

