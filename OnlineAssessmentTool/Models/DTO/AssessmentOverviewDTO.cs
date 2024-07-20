namespace OnlineAssessmentTool.Models.DTO
{
    public class AssessmentOverviewDTO
    {
        public string AssessmentName { get; set; }
        public DateTime Date { get; set; }
        public string Trainer { get; set; }
        public int NumberOfAttendees { get; set; }
        public int MaximumScore { get; set; }
        public int HighestScore { get; set; }
        public int LowestScore { get; set; }
    }
}
