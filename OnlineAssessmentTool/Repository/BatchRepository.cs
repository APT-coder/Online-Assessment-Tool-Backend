using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System;

namespace OnlineAssessmentTool.Repository
{
    public class BatchRepository : Repository<Batch>, IBatchRepository
    {
        public BatchRepository(APIContext context) : base(context)
        {

        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.batch.AnyAsync(b => b.batchid == id);
        }
    }
}