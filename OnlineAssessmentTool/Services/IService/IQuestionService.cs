using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IQuestionService
{
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<Question> AddQuestionToAssessmentAsync(int assessmentId, QuestionDTO questionDTO);
    Task<Question> UpdateQuestionAsync(int questionId, QuestionDTO questionDTO);
    Task DeleteQuestionAsync(int questionId);
}
