using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {
        Task<Assessment> GetAssessmentByIdAsync(int id);
        Task DeleteAssessmentAsync(int assessmentId);
        Task<IEnumerable<AssessmentOverviewDTO>> GetAllAssessmentOverviewsAsync();
        Task<IEnumerable<TraineeScoreDTO>> GetHighPerformersByAssessmentIdAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeScoreDTO>> GetLowPerformersByAssessmentIdAsync(int scheduledAssessmentId);
        Task<List<AssessmentTableDTO>> GetAssessmentTable();
        Task<List<TraineeAssessmentTableDTO>> GetTraineeAssessmentDetails(int scheduledAssessmentId);
    }
}
