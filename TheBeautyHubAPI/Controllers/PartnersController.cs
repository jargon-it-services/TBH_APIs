using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _service;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public PartnersController(
            IPartnerService service,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _service = service;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<PartnerResponse>> Create([FromBody] CreatePartnerRequest request)
        {
            try
            {
                var dto = _mapper.Map<CreatePartnerDto>(request);
                var result = await _service.CreateAsync(dto);
                var response = _mapper.Map<PartnerResponse>(result);
                return CreatedAtAction(nameof(GetById), new { id = response.PartnerId }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PartnerResponse>> Update(Guid id, [FromBody] UpdatePartnerRequest request)
        {
            try
            {
                var dto = _mapper.Map<UpdatePartnerDto>(request);
                var result = await _service.UpdateAsync(id, dto);
                var response = _mapper.Map<PartnerResponse>(result);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
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
        public async Task<ActionResult<PartnerResponse>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<PartnerResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerResponse>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                var response = _mapper.Map<IEnumerable<PartnerResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<PartnerResponse>>> GetByAccountId(Guid accountId)
        {
            try
            {
                var result = await _service.GetByAccountIdAsync(accountId);
                var response = _mapper.Map<IEnumerable<PartnerResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<PartnerResponse>> GetByEmail(string email)
        {
            try
            {
                var result = await _service.GetByEmailAsync(email);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<PartnerResponse>(result);
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

