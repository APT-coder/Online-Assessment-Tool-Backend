using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Services.IService
{
    public interface IAssessmentScoreService
    {
        Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs);
    }
}
