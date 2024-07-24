using AutoMapper;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Services.IService
{
    public interface IUserService
    {

        Task<bool> CreateUserAsync(
             CreateUserDTO createUserDto,
             TrainerDTO trainerDto = null,
             TraineeDTO traineeDto = null,
             List<int> batchIds = null);

        Task<List<Users>> GetUsersByRoleNameAsync(string roleName);

        public Task DeleteUserAsync(int userId);
        /*  public Task<Users> GetUserByIdAsync(int id);
          public Task<bool> UpdateUserAsync(Users user);*/
        public Task<UserDetailsDTO> GetUserDetailsByEmailAsync(string email);

        public Task<bool> UpdateUserAsync(
           CreateUserDTO createUserDto,
           TrainerDTO trainerDto = null,
           TraineeDTO traineeDto = null,
           List<int> batchIds = null);
        /*  Task<bool> CreateUserAsync(CreateUserDTO createUserDto, TrainerDTO createTrainerDto = null, TraineeDTO createTraineeDto = null);
          Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDto, TrainerDTO updateTrainerDto = null, TraineeDTO updateTraineeDto = null);*/
    }
}
