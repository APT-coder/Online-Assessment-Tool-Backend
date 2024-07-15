using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IUserRepository _userRepository;

        public UserController(APIContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // GET: api/Users/GetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No users found." }
                    });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving users from database." }
                });
            }
        }

        // GET: api/Users/GetUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "User not found." }
                    });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving user from database." }
                });
            }
        }

        // PUT: api/Users/UpdateUser/5
        // PUT: api/Users/UpdateUser/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO updateUserDto)
        {
            if (id != updateUserDto.UserID)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = new List<string> { "User ID mismatch." }
                });
            }

            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "User not found." }
                    });
                }

                // Map UpdateUserDTO to existingUser entity
                existingUser.Username = updateUserDto.Username;
                existingUser.Email = updateUserDto.Email;
                existingUser.Phone = updateUserDto.Phone;
                existingUser.isadmin = updateUserDto.IsAdmin;
                existingUser.RoleId = updateUserDto.RoleId;
                // Map other properties as needed

                await _userRepository.UpdateUserAsync(existingUser);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "User not found." }
                    });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error updating user." }
                });
            }
        }


        // POST: api/Users/CreateUser
        [HttpPost]
        public async Task<ActionResult<Users>> CreateUser(CreateUserDTO createUserDto)
        {
            try
            {
                // Map CreateUserDTO to Users entity
                var user = new Users
                {
                    Username = createUserDto.Username,
                    Email = createUserDto.Email,
                    Phone = createUserDto.Phone,
                    isadmin = createUserDto.IsAdmin,
                    RoleId = createUserDto.RoleId,
                    // Map other properties as needed
                };

                var createdUser = await _userRepository.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error creating user." }
                });
            }
        }

        // DELETE: api/Users/DeleteUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "User not found." }
                    });
                }

                await _userRepository.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error deleting user." }
                });
            }
        }

        // GET: api/Users/GetUsersByRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsersByRole()
        {
            try
            {
                var users = await _userRepository.GetUsersByRoleAsync("Trainer Manager");

                if (users == null || !users.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No users found for the role." }
                    });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving users by role from database." }
                });
            }
        }

        private bool UserExists(int id)
        {
            // Implement logic to check if user exists in database
            // Example:
            // return _context.Users.Any(u => u.UserId == id);
            return false;
        }
    }
}
