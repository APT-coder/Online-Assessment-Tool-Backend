using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IPermissionsRepository
    {

        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission> GetAsync(int id);
        Task CreateAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task RemoveAsync(Permission permission);
        Task<bool> ExistsAsync(int id);
        Task SaveAsync();
    }
}
