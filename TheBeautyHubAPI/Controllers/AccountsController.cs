#if false // unused TBH APIs
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Context;

namespace TheBeautyHubAPI.Controllers
{
    /// <summary>
    /// API Controller for Account operations.
    /// Provides CRUD endpoints for account management.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;
        private readonly BeautyHubDbContext _dbContext;

        public AccountsController(
            IAccountService accountService, 
            IExceptionLogService exceptionLogService,
            IMapper mapper,
            BeautyHubDbContext dbContext)
        {
            _accountService = accountService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet("test-db")]
        public async Task<IActionResult> TestDbConnection()
        {
            try
            {
                var connection = _dbContext.Database.GetDbConnection();
                await connection.OpenAsync();
                await connection.CloseAsync();

                return Ok(new { canConnect = true, message = "Database connection successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    canConnect = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("check-tables")]
        public async Task<IActionResult> CheckTables()
        {
            try
            {
                // Get all table names from the database
                var tables = await _dbContext.Database.SqlQuery<string>(
                    $"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'"
                ).ToListAsync();

                if (!tables.Any())
                {
                    return Ok(new
                    {
                        tablesFound = false,
                        message = "No tables found. Run migrations: dotnet ef database update",
                        tables = new List<string>()
                    });
                }

                return Ok(new
                {
                    tablesFound = true,
                    count = tables.Count,
                    tables = tables,
                    message = "Tables found successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    message = "Failed to check tables"
                });
            }
        }

        [HttpGet("test-migration")]
        public async Task<IActionResult> TestMigration()
        {
            try
            {
                var migrations = await _dbContext.Database.GetAppliedMigrationsAsync();
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

                return Ok(new
                {
                    appliedMigrations = migrations.ToList(),
                    pendingMigrations = pendingMigrations.ToList(),
                    message = "Migration info retrieved"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="request">Account creation request</param>
        /// <returns>Created account</returns>
        [HttpPost]
        public async Task<ActionResult<AccountResponse>> CreateAccount([FromBody] CreateAccountRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateAccountDto>(request);
                var accountDto = await _accountService.CreateAccountAsync(createDto);
                var response = _mapper.Map<AccountResponse>(accountDto);
                
                return CreatedAtAction(nameof(GetAccountById), new { id = response.AccountId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception to database
                await _exceptionLogService.LogExceptionAsync(ex, null, $"CreateAccount - AccountCode: {request?.AccountCode}");

                // TEMPORARY: Return detailed error for debugging
                return StatusCode(500, new
                {
                    error = "An error occurred while creating the account",
                    details = ex.Message,
                    innerError = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// Updates an existing account
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="request">Account update request</param>
        /// <returns>Updated account</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountResponse>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest request)
        {
            try
            {
                if (id != request.AccountId)
                {
                    return BadRequest(new { error = "Account ID in URL does not match request body" });
                }

                var updateDto = _mapper.Map<UpdateAccountDto>(request);
                var accountDto = await _accountService.UpdateAccountAsync(updateDto);
                var response = _mapper.Map<AccountResponse>(accountDto);
                
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception to database
                await _exceptionLogService.LogExceptionAsync(ex, null, $"UpdateAccount - AccountId: {id}");
                return StatusCode(500, new { error = "An error occurred while updating the account" });
            }
        }

        /// <summary>
        /// Deletes an account (soft delete)
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(Guid id)
        {
            try
            {
                var result = await _accountService.DeleteAccountAsync(id);
                
                if (result)
                    return NoContent();
                
                return NotFound(new { error = "Account not found" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception to database
                await _exceptionLogService.LogExceptionAsync(ex, null, $"DeleteAccount - AccountId: {id}");
                return StatusCode(500, new { error = "An error occurred while deleting the account" });
            }
        }

        /// <summary>
        /// Retrieves an account by ID
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Account details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponse>> GetAccountById(Guid id)
        {
            try
            {
                var accountDto = await _accountService.GetAccountByIdAsync(id);
                
                if (accountDto == null)
                    return NotFound(new { error = "Account not found" });
                
                var response = _mapper.Map<AccountResponse>(accountDto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception to database
                await _exceptionLogService.LogExceptionAsync(ex, null, $"GetAccountById - AccountId: {id}");
                return StatusCode(500, new { error = "An error occurred while retrieving the account" });
            }
        }

        /// <summary>
        /// Retrieves all accounts
        /// </summary>
        /// <returns>List of accounts</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAccounts()
        {
            try
            {
                var accountDtos = await _accountService.GetAllAccountsAsync();
                var response = _mapper.Map<IEnumerable<AccountResponse>>(accountDtos);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception to database
                await _exceptionLogService.LogExceptionAsync(ex, null, "GetAllAccounts");
                return StatusCode(500, new { error = "An error occurred while retrieving accounts" });
            }
        }

        /// <summary>
        /// Retrieves an account by its unique code
        /// </summary>
        /// <param name="code">Account code</param>
        /// <returns>Account details</returns>
        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<AccountResponse>> GetAccountByCode(string code)
        {
            try
            {
                var accountDto = await _accountService.GetAccountByCodeAsync(code);
                
                if (accountDto == null)
                    return NotFound(new { error = "Account not found" });
                
                var response = _mapper.Map<AccountResponse>(accountDto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception to database
                await _exceptionLogService.LogExceptionAsync(ex, null, $"GetAccountByCode - Code: {code}");
                return StatusCode(500, new { error = "An error occurred while retrieving the account" });
            }
        }
    }
}
#endif