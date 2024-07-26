using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(APIContext context) : base(context)
        {

        }
        public async Task<Trainer> GetByUserIdAsync(int userId)
        {
            return await _context.Trainers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }
        public async Task<List<string>> GetAllTrainersAsync()
        {
            return await _context.Trainers
                .Include(t => t.User)
                .Select(t => t.User.Username)
                .ToListAsync();
        }
    }
}
