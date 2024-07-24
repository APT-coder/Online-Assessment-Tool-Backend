using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace OnlineAssessmentTool.Repository
{
    public class TrainerBatchRepository : Repository<TrainerBatch>, ITrainerBatchRepository
    {
        public TrainerBatchRepository(APIContext context) : base(context)
        {

        }
        // Example method to get batches by Trainer ID
        public async Task<IEnumerable<TrainerBatch>> GetByTrainerIdAsync(int trainerId)
        {
            return await _context.TrainerBatches
                .Where(tb => tb.Trainer_id == trainerId)
                .ToListAsync();
        }
        public async Task RemoveRangeAsync(IEnumerable<TrainerBatch> entities)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }


    }
}
