using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using AutoMapper;
using OnlineAssessmentTool.Services.IService;
using Microsoft.Extensions.Logging; 

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger; 
        public UserController(IUserService userService, ILogger<UserController> logger) 
        {
            _userService = userService;
            _logger = logger; 
        }

        [HttpGet("byRole/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            try
            {
                _logger.LogInformation("Fetching users with role: {roleName}", roleName);
                var users = await _userService.GetUsersByRoleNameAsync(roleName);
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found with the role: {roleName}", roleName);
                    return NotFound(new { Message = "No users found with the given role." });
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users with role: {roleName}", roleName);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching users." });
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid user data: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                _logger.LogInformation("Creating user with data: {@request}", request);
                var result = await _userService.CreateUserAsync(request.CreateUserDTO, request.TrainerDTO, request.TraineeDTO, request.BatchIds);

                if (result)
                {
                    _logger.LogInformation("User created successfully.");
                    return Ok(new { message = "User created successfully" });
                }
                _logger.LogWarning("User creation failed.");
                return BadRequest(new { message = "User creation failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating user." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete user with ID: {id}", id);
                await _userService.DeleteUserAsync(id);
                _logger.LogInformation("User with ID {id} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting user." });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO updateUserRequestDto)
        {
            try
            {
                if (updateUserRequestDto == null)
                {
                    _logger.LogWarning("Invalid update user request data.");
                    return BadRequest("Invalid user data.");
                }
                _logger.LogInformation("Updating user with data: {@updateUserRequestDto}", updateUserRequestDto);
                var createUserDto = updateUserRequestDto.CreateUserDTO;
                TrainerDTO trainerDto = null;
                TraineeDTO traineeDto = null;
                List<int> batchIds = updateUserRequestDto.BatchIds;
                if (createUserDto.UserType == UserType.Trainer)
                {
                    trainerDto = updateUserRequestDto.TrainerDTO;
                }
                else if (createUserDto.UserType == UserType.Trainee)
                {
                    traineeDto = updateUserRequestDto.TraineeDTO;
                }
                var result = await _userService.UpdateUserAsync(createUserDto, trainerDto, traineeDto, batchIds);
                if (result)
                {
                    _logger.LogInformation("User updated successfully.");
                    return Ok(new { message = "User updated successfully." });
                }
                else
                {
                    _logger.LogWarning("User update failed.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the user." });
            }
        }

        [HttpGet("details/{email}")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            try
            {
                _logger.LogInformation("Fetching user details for email: {email}", email);
                var userDetails = await _userService.GetUserDetailsByEmailAsync(email);

                if (userDetails == null)
                {
                    _logger.LogWarning("No user details found for email: {email}", email);
                    return NotFound();
                }

                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user details for email: {email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching user details." });
            }
        }

        [HttpGet("details/{username}")]
        public async Task<IActionResult> GetUserEmailByUserName(string username)
        {
            try
            {
                _logger.LogInformation("Fetching user details for username: {username}", username);
                var userDetails = await _userService.GetUserEmailByUsernameAsync(username);

                if (userDetails == null)
                {
                    _logger.LogWarning("No user details found for username: {username}", username);
                    return NotFound();
                }

                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user details for email: {username}", username);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching user details." });
            }
        }
    }
}
