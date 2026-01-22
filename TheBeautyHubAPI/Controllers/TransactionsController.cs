using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;

namespace TheBeautyHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public TransactionsController(
            ITransactionService service,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _service = service;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> Create([FromBody] CreateTransactionRequest request)
        {
            try
            {
                var dto = _mapper.Map<CreateTransactionDto>(request);
                var result = await _service.CreateAsync(dto);
                var response = _mapper.Map<TransactionResponse>(result);
                return CreatedAtAction(nameof(GetById), new { id = response.TransactionId }, response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionResponse>> Update(Guid id, [FromBody] UpdateTransactionRequest request)
        {
            try
            {
                var dto = _mapper.Map<UpdateTransactionDto>(request);
                var result = await _service.UpdateAsync(id, dto);
                var response = _mapper.Map<TransactionResponse>(result);
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
        public async Task<ActionResult<TransactionResponse>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                var response = _mapper.Map<TransactionResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                var response = _mapper.Map<IEnumerable<TransactionResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetByAccountId(Guid accountId)
        {
            try
            {
                var result = await _service.GetByAccountIdAsync(accountId);
                var response = _mapper.Map<IEnumerable<TransactionResponse>>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("firm/{firmId}")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetByFirmId(Guid firmId)
        {
            try
            {
                var result = await _service.GetByFirmIdAsync(firmId);
                var response = _mapper.Map<IEnumerable<TransactionResponse>>(result);
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

