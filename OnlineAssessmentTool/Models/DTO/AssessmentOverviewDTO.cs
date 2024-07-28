namespace OnlineAssessmentTool.Models.DTO
{
    public class AssessmentOverviewDTO
    {
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public DateTime Date { get; set; }
        public string Trainer { get; set; }
        public string BatchName { get; set; }
        public string Status { get; set; }

        public enum AssessmentStatus
        {

            Upcoming,
            Evaluated,
            Completed,
            Cancelled

        }
    }
}
