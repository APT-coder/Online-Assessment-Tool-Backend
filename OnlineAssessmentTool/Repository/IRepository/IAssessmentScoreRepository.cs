using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IAssessmentScoreRepository : IRepository<AssessmentScore>
    {
        Task<ICollection<AssessmentScoreGraphDTO>> GetAssessmentByIdAsync(int id);
        Task<List<TraineeAssessmentScoreDTO>> GetAssessmentScoresByTraineeIdAsync(int traineeId);
    }
}
