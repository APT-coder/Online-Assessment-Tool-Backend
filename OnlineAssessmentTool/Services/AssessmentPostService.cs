using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAssessmentTool.Services
{
    public class AssessmentPostService : IAssessmentPostService
    {
        private readonly ITraineeAnswerRepository _traineeAnswerRepository;
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;

        public AssessmentPostService(ITraineeAnswerRepository traineeAnswerRepository, IAssessmentScoreRepository assessmentScoreRepository)
        {
            _traineeAnswerRepository = traineeAnswerRepository;
            _assessmentScoreRepository = assessmentScoreRepository;
        }

        public async Task<List<TraineeAnswer>> ProcessTraineeAnswers(List<PostAssessmentDTO> postAssessment, int userId)
        {
            var traineeAnswers = new List<TraineeAnswer>();
            var totalScore = 0;

            foreach (var question in postAssessment)
            {
                var questionOptions = question.QuestionOptions.FirstOrDefault();

                if (questionOptions != null)
                {
                    
                    var normalizedAnswered = question.Answered.Replace(" ", "").ToLower();
                    var normalizedCorrectAnswer = questionOptions.CorrectAnswer.Replace(" ", "").ToLower();

                    
                    var traineeAnswer = new TraineeAnswer
                    {
                        ScheduledAssessmentId = question.AssessmentId,
                        TraineeId = userId,
                        QuestionId = question.QuestionId,
                        Answer = question.Answered,
                        IsCorrect = normalizedAnswered == normalizedCorrectAnswer,
                        Score = normalizedAnswered == normalizedCorrectAnswer ? question.Points : 0
                    };
                    
                    totalScore += traineeAnswer.Score;
                    traineeAnswers.Add(traineeAnswer);
                    await _traineeAnswerRepository.AddAsync(traineeAnswer);
                }
            }

            var assessmentScore = new AssessmentScore
            {
                AvergeScore = totalScore,
                ScheduledAssessmentId = postAssessment.First().AssessmentId,
                TraineeId = userId,
                CalculatedOn = DateTime.UtcNow
            };
            await _assessmentScoreRepository.AddAsync(assessmentScore);
            await _assessmentScoreRepository.SaveAsync();

            await _traineeAnswerRepository.SaveAsync();
            return traineeAnswers;
        }
    }
}
