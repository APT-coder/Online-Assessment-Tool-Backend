namespace OnlineAssessmentTool.Models.DTO
{
    public class UpdateScoreDTO
    {
        public int ScheduledAssessmentId { get; set; }
        public int TraineeId { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
    }
}
