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
            return await _context.ScheduledAssessments
                .Where(sa => sa.AssessmentId == assessmentId)
                .GroupBy(sa => sa.BatchId)
                .Select(g => new
                {
                    BatchId = g.Key,
                    StudentCount = g.Count() // Count of students in each batch
                })
                .SumAsync(x => x.StudentCount); // Sum the counts for the given assessment

        }




    }
}

