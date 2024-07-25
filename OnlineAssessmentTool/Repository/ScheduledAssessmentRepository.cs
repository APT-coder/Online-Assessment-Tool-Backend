using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
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

            // Get the total number of students in those batches
            var totalStudents = await _context.batch
                .Where(b => batchIds.Contains(b.batchid))
                .SelectMany(b => b.Trainees)
                .CountAsync();

            return totalStudents;

        }




    }
}

