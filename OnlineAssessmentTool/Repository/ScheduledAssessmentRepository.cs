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
        /*
                public async Task AddAsync(ScheduledAssessment scheduledAssessment)
                {
                    _context.ScheduledAssessments.Add(scheduledAssessment);
                    await _context.SaveChangesAsync();
                }*/
    }
}
