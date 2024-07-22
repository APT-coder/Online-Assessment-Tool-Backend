using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IBatchRepository : IRepository<Batch>
    {
        Task<bool> ExistsAsync(int id);

    }
}
