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
        Task<Users> GetUserAsync(int userId);
        Task<TrainerDTO> GetTrainerDetailsAsync(int userId);
        Task<TraineeDTO> GetTraineeDetailsAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto);
        Task<bool> UpdateTrainerAsync(int userId, TrainerDTO trainerDto);
        Task<bool> UpdateTraineeAsync(int userId, TraineeDTO traineeDto);
        /*  Task<bool> CreateUserAsync(CreateUserDTO createUserDto, TrainerDTO createTrainerDto = null, TraineeDTO createTraineeDto = null);
          Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDto, TrainerDTO updateTrainerDto = null, TraineeDTO updateTraineeDto = null);*/
    }
}
