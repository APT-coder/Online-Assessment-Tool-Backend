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
        public Task<UserDetailsDTO> GetUserDetailsByEmailAsync(string email);
        public Task<UserDetailsDTO> GetUserEmailByUsernameAsync(string email);
        public Task<bool> UpdateUserAsync(
           CreateUserDTO createUserDto,
           TrainerDTO trainerDto = null,
           TraineeDTO traineeDto = null,
           List<int> batchIds = null);
        public Task<bool> ValidateUserAsync(string email, string password);
        public bool ValidatePassword(string hashedPassword, string providedPassword);
        public Task<bool> IsUserActive(string email);
        public Task<ApiResponse> UpdateTrainerPasswordAsync(UpdateTrainerPasswordDTO updateTrainerPasswordDTO);
    }
}
