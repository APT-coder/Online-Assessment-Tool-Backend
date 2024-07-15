using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class BatchRepository : IBatchRepository
    {
        private readonly APIContext _context;

        public BatchRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Batch>> GetAllAsync()
        {
            return await _context.batch.ToListAsync();
        }

        public async Task<Batch> GetAsync(int id)
        {
            return await _context.batch.FindAsync(id);
        }

        public async Task CreateAsync(Batch batch)
        {
            await _context.batch.AddAsync(batch);
        }

        public async Task UpdateAsync(Batch batch)
        {
            _context.batch.Update(batch);
        }

        public async Task RemoveAsync(Batch batch)
        {
            _context.batch.Remove(batch);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.batch.AnyAsync(e => e.batchid == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}