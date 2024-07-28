using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Services
{
    public class ILPIntegrationService
    {
        private readonly IIlpRepository _ilpRepository;

        public ILPIntegrationService(IIlpRepository ilpRepository)
        {
            _ilpRepository = ilpRepository;
        }

        public async Task<(double AverageScore, int TotalScore)> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId)
        {
            return await _ilpRepository.GetAverageAndTotalScore(traineeEmail, scheduledAssessmentId);
        }
    }
}
