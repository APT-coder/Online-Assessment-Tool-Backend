using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Services.IService
{
    public interface IAssessmentPostService
    {
        Task<List<TraineeAnswer>> ProcessTraineeAnswers(List<PostAssessmentDTO> postAssessment ,int userId);

    }
}
