using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;

namespace OnlineAssessmentTool.Services
{
    public class AssessmentScoreService : IAssessmentScoreService
    {
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;

        public AssessmentScoreService(IAssessmentScoreRepository assessmentScoreRepository)
        {
            _assessmentScoreRepository = assessmentScoreRepository;
        }

        public async Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs)
        {
            await _assessmentScoreRepository.UpdateAssessmentScoresAsync(assessmentScoreDTOs);
        }
    }
}
