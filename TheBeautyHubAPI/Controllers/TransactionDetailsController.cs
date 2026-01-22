using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionDetailsController : ControllerBase
    {
        private readonly ITransactionDetailService _service;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public TransactionDetailsController(
            ITransactionDetailService service,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _service = service;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDetailResponse>> Create([FromBody] CreateTransactionDetailRequest request)
        {
            try
            {
                var dto = _mapper.Map<CreateTransactionDetailDto>(request);
                var result = await _service.CreateAsync(dto);
                var response = _mapper.Map<TransactionDetailResponse>(result);
                return CreatedAtAction(nameof(GetById), new { id = response.TransactionDetailsId }, response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionDetailResponse>> Update(Guid id, [FromBody] UpdateTransactionDetailRequest request)
        {
            try
            {
                var dto = _mapper.Map<UpdateTransactionDetailDto>(request);
                var result = await _service.UpdateAsync(id, dto);
                var response = _mapper.Map<TransactionDetailResponse>(result);
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
        public async Task<ActionResult<TransactionDetailResponse>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<TransactionDetailResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<IEnumerable<TransactionDetailResponse>>> GetByTransactionId(Guid transactionId)
        {
            try
            {
                var result = await _service.GetByTransactionIdAsync(transactionId);
                var response = _mapper.Map<IEnumerable<TransactionDetailResponse>>(result);
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

