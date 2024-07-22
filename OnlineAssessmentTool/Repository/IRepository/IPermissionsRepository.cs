using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IPermissionsRepository : IRepository<Permission>
    {


        Task<bool> ExistsAsync(int id);
        public Task<List<Permission>> GetPermissionsByNamesAsync(List<string> permissionNames);
    }
}
