/*using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {
       
        *//*public AssessmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync()
        {
            return await _context.Assessments
                                 .Include(a => a.Questions)
                                 .ThenInclude(q => q.QuestionOptions)
                                 .ToListAsync();
        }
        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task AddAssessmentAsync(Assessment assessment)
        {
            if (assessment.AssessmentId == 0) 
            {
                await _context.Assessments.AddAsync(assessment);
            }
            await _context.SaveChangesAsync();
        }


       
        public async Task UpdateAssessmentAsync(Assessment assessment)
        {
            _context.Assessments.Update(assessment);
            await _context.SaveChangesAsync();
        }

       

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }

       *//*

        private readonly ApplicationDbContext _context;
        public AssessmentRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }


    }
}
*/


using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System.Threading.Tasks;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {
        private readonly APIContext _context;

        public AssessmentRepository(APIContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
