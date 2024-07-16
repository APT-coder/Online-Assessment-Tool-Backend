using OnlineAssessmentTool.Models;
using System.Collections.Generic;


namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IUserRepository:IRepository<Users>
    {
        Task<int> CreateUserAsync(Users user);
        Task<IEnumerable<Users>> GetUsersByRoleAsync(string roleName);
    }
}
