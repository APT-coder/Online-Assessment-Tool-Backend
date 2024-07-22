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
using AutoMapper;
using OnlineAssessmentTool.Services;
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
        /* private readonly ITrainerRepository _trainerRepository;
         private readonly ITraineeRepository _traineeRepository;
 */
        private readonly IMapper _mapper;
        public UserController(APIContext context, IUserRepository userRepository, IUserService userService)
        {
            _context = context;
            _userRepository = userRepository;
            _userService = userService;
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("trainer/{userId}")]
        public async Task<IActionResult> GetTrainerDetails(int userId)
        {
            var trainerDetails = await _userService.GetTrainerDetailsAsync(userId);
            if (trainerDetails == null) return NotFound();
            return Ok(trainerDetails);
        }

        [HttpGet("trainee/{userId}")]
        public async Task<IActionResult> GetTraineeDetails(int userId)
        {
            var traineeDetails = await _userService.GetTraineeDetailsAsync(userId);
            if (traineeDetails == null) return NotFound();
            return Ok(traineeDetails);
        }

        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDTO updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("trainer/{userId}")]
        public async Task<IActionResult> UpdateTrainer(int userId, [FromBody] TrainerDTO trainerDto)
        {
            var result = await _userService.UpdateTrainerAsync(userId, trainerDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("trainee/{userId}")]
        public async Task<IActionResult> UpdateTrainee(int userId, [FromBody] TraineeDTO traineeDto)
        {
            var result = await _userService.UpdateTraineeAsync(userId, traineeDto);
            if (!result) return NotFound();
            return NoContent();
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
                return NoContent(); // Returns 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
