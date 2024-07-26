using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class ScheduledAssessmentRepository : Repository<ScheduledAssessment>, IScheduledAssessmentRepository
    {

        private readonly APIContext _context;
        public ScheduledAssessmentRepository(APIContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId)
        {
            return await _context.TraineeAnswers
       .Where(ta => ta.ScheduledAssessmentId == scheduledAssessmentId)
       .Select(ta => new TraineeStatusDTO
       {
           TraineeId = ta.TraineeId,
           // Fetch Username from Users table
           Name = _context.Trainees
                         .Where(t => t.TraineeId == ta.TraineeId)
                         .Select(t => t.User.Username)
                         .FirstOrDefault(),
           Score = _context.AssessmentScores
                         .Where(a => a.TraineeId == ta.TraineeId && a.ScheduledAssessmentId == ta.ScheduledAssessmentId)
                         .Select(a => a.AvergeScore)
                         .FirstOrDefault()
       })
       .Distinct()
       .ToListAsync();
        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId)
        {
            // Retrieve the batch ID for the given scheduled assessment
            var batchId = await _context.ScheduledAssessments
                .Where(sa => sa.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(sa => sa.BatchId)
                .FirstOrDefaultAsync();

            var attendedTraineeIds = await _context.TraineeAnswers
                .Where(ta => ta.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(ta => ta.TraineeId)
                .ToListAsync();

            var absentTrainees = await _context.Trainees
                .Where(t => t.BatchId == batchId && !attendedTraineeIds.Contains(t.TraineeId))
                .Select(t => new TraineeStatusDTO
                {
                    TraineeId = t.TraineeId,
                    Name = t.User.Username
                })
                .ToListAsync();

            return absentTrainees;
        }

        public async Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId)
        {
            var result = await _context.TraineeAnswers
                .Where(ta => ta.TraineeId == traineeId && ta.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(ta => new
                {
                    ta.QuestionId,
                    ta.Answer,
                    ta.IsCorrect,
                    ta.Score,
                    QuestionText = _context.Questions
                        .Where(q => q.QuestionId == ta.QuestionId)
                        .Select(q => q.QuestionText)
                        .FirstOrDefault(),
                    QuestionType = _context.Questions
                        .Where(q => q.QuestionId == ta.QuestionId)
                        .Select(q => q.QuestionType)
                        .FirstOrDefault(),
                    Points = _context.Questions
                        .Where(q => q.QuestionId == ta.QuestionId)
                        .Select(q => q.Points)
                        .FirstOrDefault(),
                    QuestionOptions = _context.QuestionOptions
                        .Where(o => o.QuestionId == ta.QuestionId)
                        .Select(o => new QuestionOptionDTO
                        {
                            Option1 = o.Option1,
                            Option2 = o.Option2,
                            Option3 = o.Option3,
                            Option4 = o.Option4,
                            CorrectAnswer = o.CorrectAnswer
                        })
                        .FirstOrDefault() // Assuming you need the options for each question
                })
                .ToListAsync();

            return result.Select(item => new TraineeAnswerDetailDTO
            {
                QuestionId = item.QuestionId,
                Answer = item.Answer,
                IsCorrect = item.IsCorrect,
                Score = item.Score,
                QuestionText = item.QuestionText,
                QuestionType = item.QuestionType,
                Points = item.Points,
                QuestionOptions = item.QuestionOptions
            });
        }
        public async Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId)
        {
            var batchIds = await _context.ScheduledAssessments
                .Where(sa => sa.AssessmentId == assessmentId)
                .Select(sa => sa.BatchId)
                .Distinct()
                .ToListAsync();

            var totalStudents = await _context.batch
                .Where(b => batchIds.Contains(b.batchid))
                .SelectMany(b => b.Trainees)
                .CountAsync();

            return totalStudents;
        }

        public async Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId)
        {
            var traineeBatchIds = await _context.Trainees
                .Where(t => t.UserId == userId)
                .Select(t => t.BatchId)
                .ToListAsync();

            var scheduledAssessments = await _context.ScheduledAssessments
                .Where(sa => traineeBatchIds.Contains(sa.BatchId))
                .Select(sa => new GetScheduledAssessmentDTO
                {
                    BatchId = sa.BatchId,
                    AssessmentId = sa.AssessmentId,
                    AssessmentName = _context.Assessments
                        .Where(a => a.AssessmentId == sa.AssessmentId)
                        .Select(a => a.AssessmentName)
                        .FirstOrDefault(),
                    ScheduledDate = sa.ScheduledDate,
                    AssessmentDuration = sa.AssessmentDuration,
                    StartDate = sa.StartDate,
                    EndDate = sa.EndDate,
                    StartTime = sa.StartTime,
                    EndTime = sa.EndTime,
                    Status = sa.Status,
                    CanRandomizeQuestion = sa.CanRandomizeQuestion,
                    CanDisplayResult = sa.CanDisplayResult,
                    CanSubmitBeforeEnd = sa.CanSubmitBeforeEnd
                })
                .ToListAsync();

            return scheduledAssessments;
        }
    }
}

