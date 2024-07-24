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
                return NoContent(); // Returns 204 No Content
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

            // Map CreateUserDTO to appropriate DTOs
            var createUserDto = updateUserRequestDto.CreateUserDTO;

            TrainerDTO trainerDto = null;
            TraineeDTO traineeDto = null;
            List<int> batchIds = updateUserRequestDto.BatchIds;

            if (createUserDto.UserType == UserType.Trainer)
            {
                trainerDto = updateUserRequestDto.TrainerDTO;
                // No need to manually extract batch IDs as they are already provided
            }
            else if (createUserDto.UserType == UserType.Trainee)
            {
                traineeDto = updateUserRequestDto.TraineeDTO;
                // No batch IDs needed for trainees
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


        /*  [HttpPut("{id}")]
          public async Task<IActionResult> UpdateUser([FromBody] Users user)
          {
              var result = await _userService.UpdateUserAsync(user);

              if (result == null)
              {
                  return NotFound();
              }

              // Retrieve the updated user with detailed info
              var updatedUser = await _userService.GetUserByIdAsync(user.UserId);

              if (updatedUser.UserType == UserType.Trainer)
              {
                  var trainer = await _trainerRepository.GetByUserIdAsync(user.UserId);
                  if (trainer != null)
                  {
                      return Ok(new
                      {
                          User = updatedUser,
                          Trainer = trainer
                      });
                  }
              }
              else if (updatedUser.UserType == UserType.Trainee)
              {
                  var trainee = await _traineeRepository.GetByUserIdAsync(user.UserId);
                  if (trainee != null)
                  {
                      return Ok(new
                      {
                          User = updatedUser,
                          Trainee = trainee
                      });
                  }
              }

              // Return only user details if no specific details are found
              return Ok(updatedUser);
          }
    */
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

