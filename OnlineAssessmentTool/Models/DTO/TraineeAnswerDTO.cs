using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAssessmentTool.Models.DTO
{
    public class TraineeAnswerDTO
    {
        public int ScheduledAssessmentId { get; set; }


        public int TraineeId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
    }
}
