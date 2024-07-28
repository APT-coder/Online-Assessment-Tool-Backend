using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IIlpRepository
    {

        Task<(double AverageScore, int TotalScore)> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId);

    }
}

