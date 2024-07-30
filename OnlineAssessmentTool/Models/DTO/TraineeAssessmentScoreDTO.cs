namespace OnlineAssessmentTool.Models.DTO
{
    public class TraineeAssessmentScoreDTO
    {
        public int AssessmentScoreId { get; set; }
        public int ScheduledAssessmentId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int Score { get; set; }
        public DateTime CalculatedOn { get; set; }
    }
}
