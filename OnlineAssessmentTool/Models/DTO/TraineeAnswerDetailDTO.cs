namespace OnlineAssessmentTool.Models.DTO
{
    public class TraineeAnswerDetailDTO
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // Add this
        public int Points {  get; set; }
        public QuestionOptionDTO QuestionOptions { get; set; } // Add this
    }
}
