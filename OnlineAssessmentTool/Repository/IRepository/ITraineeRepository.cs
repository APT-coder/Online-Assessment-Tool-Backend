using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface ITraineeRepository : IRepository<Trainee>
    {
        public Task<Trainee> GetByUserIdAsync(int userId);
    }
}
