namespace OnlineAssessmentTool.Models.DTO
{
    public class GetScheduledAssessmentDTO
    {
        public int BatchId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan AssessmentDuration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public AssessmentStatus Status { get; set; }

        public bool CanRandomizeQuestion { get; set; }
        public bool CanDisplayResult { get; set; }
        public bool CanSubmitBeforeEnd { get; set; }
    }
}
