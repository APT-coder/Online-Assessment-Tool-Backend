using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IScheduledAssessmentRepository : IRepository<ScheduledAssessment>
    {
        Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId);
        Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId);

    }
}
