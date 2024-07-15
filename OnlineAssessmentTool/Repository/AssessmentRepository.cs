using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly APIContext _context;

        public AssessmentRepository(APIContext context)
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


        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions.Include(q => q.QuestionOptions)
                                           .FirstOrDefaultAsync(q => q.QuestionId == questionId);
        }

        public async Task UpdateAssessmentAsync(Assessment assessment)
        {
            _context.Assessments.Update(assessment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Questions.Update(question);
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

        public async Task DeleteQuestionAsync(int questionId)
        {
            var question = await _context.Questions.Include(q => q.QuestionOptions)
                                                   .FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

    }
}
