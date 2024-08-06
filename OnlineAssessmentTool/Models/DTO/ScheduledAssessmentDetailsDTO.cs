namespace OnlineAssessmentTool.Models.DTO
{
    public class ScheduledAssessmentDetailsDTO
    {
        public int ScheduledAssessmentId { get; set; }
        public int MaximumScore { get; set; }
        public int TotalTrainees { get; set; }
        public int TraineesAttended { get; set; }
        public int Absentees { get; set; }
        public DateTime AssessmentDate { get; set; }
    }

}
