using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IPermissionsRepository : IRepository<Permission>
    {


        Task<bool> ExistsAsync(int id);

    }
}
