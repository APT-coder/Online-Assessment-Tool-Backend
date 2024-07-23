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
    }
}
