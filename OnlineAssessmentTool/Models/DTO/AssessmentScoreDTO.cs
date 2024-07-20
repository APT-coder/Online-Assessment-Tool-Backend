namespace OnlineAssessmentTool.Models.DTO
{
    public class AssessmentScoreDTO
    {
        public int ScheduledAssessmentId { get; set; }
        public int TraineeId { get; set; }
        public int AvergeScore { get; set; }
        public DateTime CalculatedOn { get; set; }
    }
}
