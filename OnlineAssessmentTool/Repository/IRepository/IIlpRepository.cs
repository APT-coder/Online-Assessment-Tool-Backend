using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IIlpRepository
    {

        Task<(double AverageScore, int TotalScore)> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId);

        Task<IlpIntegrationScheduledAssessmentDTO> GetScheduledAssessmentDetails(int scheduledAssessmentId);

    }
}
