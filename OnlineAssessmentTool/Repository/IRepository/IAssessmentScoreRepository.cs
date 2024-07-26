using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IAssessmentScoreRepository : IRepository<AssessmentScore>
    {
        Task<ICollection<AssessmentScoreGraphDTO>> GetAssessmentByIdAsync(int id);
        Task<AssessmentScore> GetByScheduledAssessmentAndTraineeAsync(int scheduledAssessmentId, int traineeId);
        Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs);
    }
}
