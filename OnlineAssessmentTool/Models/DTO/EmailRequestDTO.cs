namespace OnlineAssessmentTool.Models.DTO
{
    public class EmailRequestDTO
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<AttachmentDTO> Attachments { get; set; } // Added property for attachments
    }

    public class AttachmentDTO
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }
}
