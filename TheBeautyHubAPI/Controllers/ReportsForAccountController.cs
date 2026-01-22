using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsForAccountController : ControllerBase
    {
        private readonly IReportForAccountService _service;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public ReportsForAccountController(
            IReportForAccountService service,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _service = service;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ReportForAccountResponse>> Create([FromBody] CreateReportForAccountRequest request)
        {
            try
            {
                var dto = _mapper.Map<CreateReportForAccountDto>(request);
                var result = await _service.CreateAsync(dto);
                var response = _mapper.Map<ReportForAccountResponse>(result);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReportForAccountResponse>> Update(Guid id, [FromBody] UpdateReportForAccountRequest request)
        {
            try
            {
                var dto = _mapper.Map<UpdateReportForAccountDto>(request);
                var result = await _service.UpdateAsync(id, dto);
                var response = _mapper.Map<ReportForAccountResponse>(result);
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
        public async Task<ActionResult<ReportForAccountResponse>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<ReportForAccountResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<ReportForAccountResponse>>> GetByAccountId(Guid accountId)
        {
            try
            {
                var result = await _service.GetByAccountIdAsync(accountId);
                var response = _mapper.Map<IEnumerable<ReportForAccountResponse>>(result);
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

