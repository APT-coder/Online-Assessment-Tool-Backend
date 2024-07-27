using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentScoreRepository : Repository<AssessmentScore>, IAssessmentScoreRepository
    {
        public AssessmentScoreRepository(APIContext context) : base(context) { }

        public async Task<ICollection<AssessmentScoreGraphDTO>> GetAssessmentByIdAsync(int id)
        {
            var assessmentScores = await _context.AssessmentScores
                                      .Where(a => a.ScheduledAssessmentId == id)
                                      .Select(a => new AssessmentScoreGraphDTO
                                      {
                                          TraineeId = a.TraineeId,
                                          AvergeScore = a.AvergeScore
                                      })
                                      .ToListAsync();
            return assessmentScores;
        }

        public async Task<AssessmentScore> GetByScheduledAssessmentAndTraineeAsync(int scheduledAssessmentId, int traineeId)
        {
            return await _context.AssessmentScores
                .FirstOrDefaultAsync(a => a.ScheduledAssessmentId == scheduledAssessmentId && a.TraineeId == traineeId);
        }

        public async Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs)
        {
            foreach (var assessmentScoreDTO in assessmentScoreDTOs)
            {
                var assessmentScore = await GetByScheduledAssessmentAndTraineeAsync(assessmentScoreDTO.ScheduledAssessmentId, assessmentScoreDTO.TraineeId);

                if (assessmentScore == null)
                {
                    // If the assessment score record doesn't exist, continue to the next one
                    continue;
                }

                // Update the assessment score
                assessmentScore.AvergeScore = assessmentScoreDTO.AvergeScore;
                assessmentScore.CalculatedOn = DateTime.UtcNow;

                _context.AssessmentScores.Update(assessmentScore);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetScoreDistributionAsync(int assessmentId)
        {
            var scoreDistribution = await _context.AssessmentScores
                .Where(a => a.ScheduledAssessmentId == assessmentId)
                .GroupBy(a => new
                {
                    Category = a.AvergeScore >= 90 ? "Above 90" :
                               a.AvergeScore >= 80 ? "80 - 90" :
                               a.AvergeScore >= 70 ? "70 - 80" :
                               a.AvergeScore >= 60 ? "60 - 70" : "Below 60"
                })
                .Select(g => new
                {
                    Category = g.Key.Category,
                    Count = g.Count()
                })
                .ToListAsync();

            return scoreDistribution;
        }
    }
}
