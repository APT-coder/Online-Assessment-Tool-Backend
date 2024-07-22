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

        [HttpGet("details/{email}")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            var userDetails = await _userService.GetUserDetailsByEmailAsync(email);
            if (userDetails == null)
                return NotFound();

            return Ok(userDetails);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Users user)
        {
            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch");
            }

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            bool result = await _userService.UpdateUserAsync(user);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the user");
            }
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
