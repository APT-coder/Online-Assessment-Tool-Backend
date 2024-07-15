using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IBatchRepository
    {
        Task<IEnumerable<Batch>> GetAllAsync();
        Task<Batch> GetAsync(int id);
        Task CreateAsync(Batch batch);
        Task UpdateAsync(Batch batch);
        Task RemoveAsync(Batch batch);
        Task<bool> ExistsAsync(int id);
        Task SaveAsync();
    }
}
