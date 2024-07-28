using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface ITraineeAnswerRepository : IRepository<TraineeAnswer>
    {
        Task<TraineeAnswer> GetTraineeAnswerAsync(int scheduledAssessmentId, int traineeId, int questionId);
        Task UpdateTraineeAnswerAsync(TraineeAnswer traineeAnswer);
        Task<bool> CheckTraineeAnswerExistsAsync(int scheduledAssessmentId, int userId);
    }
}
