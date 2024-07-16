using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IRoleRepository: IRepository<Role>
    {
       
        bool RoleExists(int id);
    }
}
