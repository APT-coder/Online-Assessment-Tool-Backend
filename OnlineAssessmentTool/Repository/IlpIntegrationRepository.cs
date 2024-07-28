using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class IlpIntegrationRepository : IIlpRepository
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ITraineeRepository _traineeRepository;
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;
        private readonly APIContext _dbContext;

        public IlpIntegrationRepository(IAssessmentRepository assessmentRepository,
                                            ITraineeRepository traineeRepository,
                                            IAssessmentScoreRepository assessmentScoreRepository,
                                             APIContext dbContext)
        {
            _assessmentRepository = assessmentRepository;
            _traineeRepository = traineeRepository;
            _assessmentScoreRepository = assessmentScoreRepository;
            _dbContext = dbContext;
        }

        public async Task<(double AverageScore, int TotalScore)> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId)
        {
            var trainee = await _dbContext.Trainees.Include(t => t.User)
                                                  .FirstOrDefaultAsync(t => t.User.Email == traineeEmail);
            if (trainee == null)
            {
                return (0, 0);
            }

            var assessmentScore = await _dbContext.AssessmentScores
                                                .FirstOrDefaultAsync(a => a.TraineeId == trainee.TraineeId &&
                                                                          a.ScheduledAssessmentId == scheduledAssessmentId);
            if (assessmentScore == null)
            {
                return (0, 0);
            }

            var assessment = await _dbContext.Assessments
                                           .FirstOrDefaultAsync(a => a.AssessmentId == scheduledAssessmentId);
            if (assessment == null)
            {
                return (0, 0);
            }

            return (assessmentScore.AvergeScore, assessment.TotalScore ?? 0);
        }
    }
}
