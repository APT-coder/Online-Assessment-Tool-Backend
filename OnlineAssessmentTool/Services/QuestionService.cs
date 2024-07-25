using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using AutoMapper;

public class QuestionService : IQuestionService
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public QuestionService(IAssessmentRepository assessmentRepository, IMapper mapper, IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
        _assessmentRepository = assessmentRepository;
        _mapper = mapper;
    }

    public async Task<Question> GetQuestionByIdAsync(int questionId)
    {
        return await _questionRepository.GetQuestionByIdAsync(questionId);
    }

    public async Task<Question> AddQuestionToAssessmentAsync(int assessmentId, QuestionDTO questionDTO)
    {
        var question = _mapper.Map<Question>(questionDTO);
        question.CreatedOn = DateTime.UtcNow;
        question.AssessmentId = assessmentId;

        var assessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
        if (assessment != null)
        {

            assessment.Questions.Add(question);
            await _assessmentRepository.UpdateAsync(assessment);
            await _assessmentRepository.SaveAsync();

            return question;
        }

        return null;
    }

    public async Task<Question> UpdateQuestionAsync(int questionId, QuestionDTO questionDTO)
    {
        var existingQuestion = await _questionRepository.GetQuestionByIdAsync(questionId);

        if (existingQuestion != null)
        {
            existingQuestion.QuestionText = questionDTO.QuestionText;
            existingQuestion.QuestionType = questionDTO.QuestionType;
            existingQuestion.Points = questionDTO.Points;

            var existingOptions = existingQuestion.QuestionOptions.ToList();

            foreach (var dtoOption in questionDTO.QuestionOptions)
            {

                var optionId = existingQuestion.QuestionOptions.FirstOrDefault()?.QuestionOptionId;
                var existingOption = existingOptions.FirstOrDefault(o => o.QuestionOptionId == optionId);

                existingOption.Option1 = dtoOption.Option1;
                existingOption.Option2 = dtoOption.Option2;
                existingOption.Option3 = dtoOption.Option3;
                existingOption.Option4 = dtoOption.Option4;
                existingOption.CorrectAnswer = dtoOption.CorrectAnswer;
            }

            await _questionRepository.UpdateQuestionAsync(existingQuestion);
            await _questionRepository.SaveAsync();
            return existingQuestion;
        }
        return null;
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        await _questionRepository.DeleteQuestionAsync(questionId);
    }
}