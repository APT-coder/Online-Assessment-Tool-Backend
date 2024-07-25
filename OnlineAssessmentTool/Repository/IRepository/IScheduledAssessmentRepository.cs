using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IScheduledAssessmentRepository : IRepository<ScheduledAssessment>
    {
        Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId);

    }
}
