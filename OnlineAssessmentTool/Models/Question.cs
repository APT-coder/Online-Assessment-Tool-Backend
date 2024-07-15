namespace OnlineAssessmentTool.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Assessment Assessment { get; set; }
        public ICollection<QuestionOption> QuestionOptions { get; set; }
    }
}
