using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(int questionId);
    }
}
