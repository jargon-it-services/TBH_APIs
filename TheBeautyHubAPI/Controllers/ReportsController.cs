using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public ReportsController(
            IReportService service,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _service = service;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ReportResponse>> Create([FromBody] CreateReportRequest request)
        {
            try
            {
                var dto = _mapper.Map<CreateReportDto>(request);
                var result = await _service.CreateAsync(dto);
                var response = _mapper.Map<ReportResponse>(result);
                return CreatedAtAction(nameof(GetById), new { id = response.ReportId }, response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReportResponse>> Update(Guid id, [FromBody] UpdateReportRequest request)
        {
            try
            {
                var dto = _mapper.Map<UpdateReportDto>(request);
                var result = await _service.UpdateAsync(id, dto);
                var response = _mapper.Map<ReportResponse>(result);
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
        public async Task<ActionResult<ReportResponse>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<ReportResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportResponse>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                var response = _mapper.Map<IEnumerable<ReportResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ReportResponse>>> GetActive()
        {
            try
            {
                var result = await _service.GetActiveReportsAsync();
                var response = _mapper.Map<IEnumerable<ReportResponse>>(result);
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

