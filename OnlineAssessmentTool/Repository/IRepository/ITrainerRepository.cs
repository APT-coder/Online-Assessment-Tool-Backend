using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface ITrainerRepository : IRepository<Trainer>
    {
        public Task<Trainer> GetByUserIdAsync(int userId);
    }
}
