using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IScheduledAssessmentRepository : IRepository<ScheduledAssessment>
    {
        /* Task AddAsync(ScheduledAssessment scheduledAssessment);*/
        Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId);

    }
}
