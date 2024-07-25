using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class TraineeRepository : Repository<Trainee>, ITraineeRepository
    {
        public TraineeRepository(APIContext context) : base(context)
        {

        }

        public async Task<Trainee> GetByUserIdAsync(int userId)
        {
            return await _context.Trainees
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

    }
}
