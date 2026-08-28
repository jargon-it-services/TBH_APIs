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
    /// API Controller for User operations.
    /// Provides CRUD endpoints for user management.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IExceptionLogService _exceptionLogService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService userService,
            IExceptionLogService exceptionLogService,
            IMapper mapper)
        {
            _userService = userService;
            _exceptionLogService = exceptionLogService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="request">User creation request</param>
        /// <returns>Created user</returns>
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                var createDto = _mapper.Map<CreateUserDto>(request);
                var userDto = await _userService.CreateUserAsync(createDto);
                var response = _mapper.Map<UserResponse>(userDto);
                
                return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
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
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while creating the user" });
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="request">User update request</param>
        /// <returns>Updated user</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (id != request.UserId)
                {
                    return BadRequest(new { error = "User ID in URL does not match request body" });
                }

                var updateDto = _mapper.Map<UpdateUserDto>(request);
                var userDto = await _userService.UpdateUserAsync(updateDto);
                var response = _mapper.Map<UserResponse>(userDto);
                
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
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while updating the user" });
            }
        }

        /// <summary>
        /// Deletes a user (soft delete)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                
                if (result)
                    return NoContent();
                
                return NotFound(new { error = "User not found" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while deleting the user" });
            }
        }

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
        {
            try
            {
                var userDto = await _userService.GetUserByIdAsync(id);
                
                if (userDto == null)
                    return NotFound(new { error = "User not found" });
                
                var response = _mapper.Map<UserResponse>(userDto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while retrieving the user" });
            }
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            try
            {
                var userDtos = await _userService.GetAllUsersAsync();
                var response = _mapper.Map<IEnumerable<UserResponse>>(userDtos);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while retrieving users" });
            }
        }

        /// <summary>
        /// Retrieves all users for a specific account
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <returns>List of users</returns>
        [HttpGet("by-account/{accountId}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersByAccountId(Guid accountId)
        {
            try
            {
                var userDtos = await _userService.GetUsersByAccountIdAsync(accountId);
                var response = _mapper.Map<IEnumerable<UserResponse>>(userDtos);
                
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while retrieving users" });
            }
        }

        /// <summary>
        /// Retrieves a user by email
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User details</returns>
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail(string email)
        {
            try
            {
                var userDto = await _userService.GetUserByEmailAsync(email);
                
                if (userDto == null)
                    return NotFound(new { error = "User not found" });
                
                var response = _mapper.Map<UserResponse>(userDto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while retrieving the user" });
            }
        }

        /// <summary>
        /// Retrieves all users managed by a specific manager
        /// </summary>
        /// <param name="managerId">Manager ID</param>
        /// <returns>List of users</returns>
        [HttpGet("by-manager/{managerId}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersByManagerId(Guid managerId)
        {
            try
            {
                var userDtos = await _userService.GetUsersByManagerIdAsync(managerId);
                var response = _mapper.Map<IEnumerable<UserResponse>>(userDtos);
                
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await _exceptionLogService.LogExceptionAsync(ex);
                return StatusCode(500, new { error = "An error occurred while retrieving users" });
            }
        }
    }
}
#endif
