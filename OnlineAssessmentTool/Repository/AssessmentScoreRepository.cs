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

        public async Task<List<TraineeAssessmentScoreDTO>> GetAssessmentScoresByTraineeIdAsync(int traineeId)
        {
            return await _context.AssessmentScores
                .Where(ass => ass.TraineeId == traineeId)
                .Select(ass => new TraineeAssessmentScoreDTO
                {
                    AssessmentScoreId = ass.AssessmentScoreId,
                    ScheduledAssessmentId = ass.ScheduledAssessmentId,
                    AssessmentName = _context.Assessments
                        .Where(a => a.AssessmentId == ass.ScheduledAssessmentId)
                        .Select(a => a.AssessmentName)
                        .FirstOrDefault(),

                    ScheduledDate =_context.ScheduledAssessments
                        .Where(s => s.ScheduledAssessmentId == ass.ScheduledAssessmentId)
                        .Select(s => s.ScheduledDate)
                        .FirstOrDefault(),

                    /*ScheduledDate = a.ScheduledAssessment.StartDate,*/
                    Score = ass.AvergeScore,
                    CalculatedOn = ass.CalculatedOn
                })
                .ToListAsync();
        }

    }
}
