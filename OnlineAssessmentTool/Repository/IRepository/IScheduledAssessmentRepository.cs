using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IScheduledAssessmentRepository : IRepository<ScheduledAssessment>
    {
        Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId);
        Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId);
        Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId);
        Task<AssessmentTableDTO> GetAssessmentTableByScheduledAssessmentId(int scheduledAssessmentId);
    }
}
