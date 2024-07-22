using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {

        bool RoleExists(int id);

        public Task<Role> GetByIdAsync(int id);
        //  public  Task<Role> GetRoleWithPermissionsAsync(int roleId);
    }


}
