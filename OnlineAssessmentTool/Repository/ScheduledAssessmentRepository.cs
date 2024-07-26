using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class ScheduledAssessmentRepository : Repository<ScheduledAssessment>, IScheduledAssessmentRepository
    {
        public ScheduledAssessmentRepository(APIContext context) : base(context)
        {


        }

        public async Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId)
        {
            var batchIds = await _context.ScheduledAssessments
                .Where(sa => sa.AssessmentId == assessmentId)
                .Select(sa => sa.BatchId)
                .Distinct()
                .ToListAsync();

            var totalStudents = await _context.batch
                .Where(b => batchIds.Contains(b.batchid))
                .SelectMany(b => b.Trainees)
                .CountAsync();

            return totalStudents;
        }

        public async Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId)
        {
            var traineeBatchIds = await _context.Trainees
                .Where(t => t.UserId == userId)
                .Select(t => t.BatchId)
                .ToListAsync();

            var scheduledAssessments = await _context.ScheduledAssessments
                .Where(sa => traineeBatchIds.Contains(sa.BatchId))
                .Select(sa => new GetScheduledAssessmentDTO
                {
                    BatchId = sa.BatchId,
                    AssessmentId = sa.AssessmentId,
                    AssessmentName = _context.Assessments
                        .Where(a => a.AssessmentId == sa.AssessmentId)
                        .Select(a => a.AssessmentName)
                        .FirstOrDefault(),
                    ScheduledDate = sa.ScheduledDate,
                    AssessmentDuration = sa.AssessmentDuration,
                    StartDate = sa.StartDate,
                    EndDate = sa.EndDate,
                    StartTime = sa.StartTime,
                    EndTime = sa.EndTime,
                    Status = sa.Status,
                    CanRandomizeQuestion = sa.CanRandomizeQuestion,
                    CanDisplayResult = sa.CanDisplayResult,
                    CanSubmitBeforeEnd = sa.CanSubmitBeforeEnd
                })
                .ToListAsync();

            return scheduledAssessments;
        }
    }
}

