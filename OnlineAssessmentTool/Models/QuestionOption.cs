namespace OnlineAssessmentTool.Models
{
    public class QuestionOption
    {
        public int QuestionOptionId { get; set; }
        public int QuestionId { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string CorrectAnswer { get; set; }
        public Question Question { get; set; }
    }
}
