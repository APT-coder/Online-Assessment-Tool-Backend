using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;

namespace OnlineAssessmentTool.Services
{
    public class ScheduledAssessmentService : IScheduledAssessmentService
    {
        private readonly IScheduledAssessmentRepository _scheduledAssessmentRepository;

        public ScheduledAssessmentService(IScheduledAssessmentRepository scheduledAssessmentRepository)
        {
            _scheduledAssessmentRepository = scheduledAssessmentRepository;
        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetAttendedStudentsAsync(scheduledAssessmentId);
        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetAbsentStudentsAsync(scheduledAssessmentId);
        }
        public async Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetTraineeAnswerDetailsAsync(traineeId, scheduledAssessmentId);
        }
    }
}
