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
    /// API Controller for Wallet management.
    /// Provides endpoints for CRUD operations on wallets.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public WalletsController(
            IWalletService walletService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _walletService = walletService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new wallet.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<WalletResponse>> CreateWallet([FromBody] CreateWalletRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateWalletDto>(request);
                var walletDto = await _walletService.CreateWalletAsync(createDto);
                var response = _mapper.Map<WalletResponse>(walletDto);
                return CreatedAtAction(nameof(GetWalletById), new { id = response.WalletId }, response);
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
        /// Updates an existing wallet.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<WalletResponse>> UpdateWallet(Guid id, [FromBody] UpdateWalletRequest request)
        {
            try
            {
                var updateDto = _mapper.Map<UpdateWalletDto>(request);
                updateDto.WalletId = id;
                var walletDto = await _walletService.UpdateWalletAsync(updateDto);
                var response = _mapper.Map<WalletResponse>(walletDto);
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
        /// Deletes a wallet by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWallet(Guid id)
        {
            try
            {
                await _walletService.DeleteWalletAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets a wallet by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<WalletResponse>> GetWalletById(Guid id)
        {
            var walletDto = await _walletService.GetWalletByIdAsync(id);
            if (walletDto == null)
                return NotFound($"Wallet with ID {id} not found.");

            var response = _mapper.Map<WalletResponse>(walletDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all wallets for a specific account.
        /// </summary>
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<WalletResponse>>> GetWalletsByAccountId(Guid accountId)
        {
            var walletsDto = await _walletService.GetWalletsByAccountIdAsync(accountId);
            var response = _mapper.Map<IEnumerable<WalletResponse>>(walletsDto);
            return Ok(response);
        }

        /// <summary>
        /// Gets all wallets.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletResponse>>> GetAllWallets()
        {
            var walletsDto = await _walletService.GetAllWalletsAsync();
            var response = _mapper.Map<IEnumerable<WalletResponse>>(walletsDto);
            return Ok(response);
        }
    }
}
#endif
