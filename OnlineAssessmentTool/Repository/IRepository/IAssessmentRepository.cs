using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IAssessmentRepository
    {
        Task<IEnumerable<Assessment>> GetAllAssessmentsAsync();
        Task<Assessment> GetAssessmentByIdAsync(int id);
        Task AddAssessmentAsync(Assessment assessment);

        Task<Question> GetQuestionByIdAsync(int questionId);

        Task UpdateAssessmentAsync(Assessment assessment);

        Task UpdateQuestionAsync(Question question);

        Task DeleteQuestionAsync(int questionId);
        Task DeleteAssessmentAsync(int assessmentId);
    }
}
