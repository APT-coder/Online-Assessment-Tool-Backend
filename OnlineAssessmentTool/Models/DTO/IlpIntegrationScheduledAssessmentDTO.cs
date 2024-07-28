namespace OnlineAssessmentTool.Models.DTO
{
    public class IlpIntegrationScheduledAssessmentDTO
    {
        public string BatchName { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string CreatedByName { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string Link { get; set; }
        public AssessmentStatus Status { get; set; }
        public string StatusString => Status.ToString();
    }
}
