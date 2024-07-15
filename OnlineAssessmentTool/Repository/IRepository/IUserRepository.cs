using OnlineAssessmentTool.Models;
using System.Collections.Generic;


namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetUsersAsync();
        Task<Users> GetUserByIdAsync(int id);
        Task<Users> CreateUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<Users>> GetUsersByRoleAsync(string roleName);
    }
}
