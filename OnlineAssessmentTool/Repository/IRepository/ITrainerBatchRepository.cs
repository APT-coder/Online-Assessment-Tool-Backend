using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface ITrainerBatchRepository : IRepository<TrainerBatch>
    {
        public Task<IEnumerable<TrainerBatch>> GetByTrainerIdAsync(int trainerId);
        Task RemoveRangeAsync(IEnumerable<TrainerBatch> entities);
    }
}
