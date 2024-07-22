namespace OnlineAssessmentTool.Models.DTO
{
    public class AssessmentTableDTO
    {
        public string AssessmentName { get; set; }
        public string BatchName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
        public enum AssessmentStatus
        {
            InProgress,
            Completed,
            Upcoming,
            Cancelled
        }
    }
}
