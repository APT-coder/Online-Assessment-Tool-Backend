using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using AutoMapper;
using OnlineAssessmentTool.Services.IService;

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ITrainerRepository _trainerRepository;
        private readonly ITraineeRepository _traineeRepository;
        private readonly IMapper _mapper;
        public UserController(APIContext context, IUserRepository userRepository, IUserService userService, ITraineeRepository traineeRepository, ITrainerRepository trainerRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _userService = userService;
            _traineeRepository = traineeRepository;
            _trainerRepository = trainerRepository;
        }

        [HttpGet("byRole/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            var users = await _userService.GetUsersByRoleNameAsync(roleName);
            if (users == null || !users.Any())
            {
                return NotFound(new { Message = "No users found with the given role." });
            }

            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            var result = await _userService.CreateUserAsync(request.CreateUserDTO, request.TrainerDTO, request.TraineeDTO, request.BatchIds);

            if (result)
            {
                return Ok(new { message = "User created successfully" });
            }
            return BadRequest(new { message = "User creation failed" });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO updateUserRequestDto)
        {
            if (updateUserRequestDto == null)
            {
                return BadRequest("Invalid user data.");
            }

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

            var result = await _userService.UpdateUserAsync(
                createUserDto,
                trainerDto,
                traineeDto,
                batchIds
            );

            if (result)
            {
                return Ok(new { message = "User updated successfully." });
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpGet("details/{email}")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            var userDetails = await _userService.GetUserDetailsByEmailAsync(email);
            if (userDetails == null)
                return NotFound();

            return Ok(userDetails);
        }
    }
}

