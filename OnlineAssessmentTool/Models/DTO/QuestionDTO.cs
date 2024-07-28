namespace OnlineAssessmentTool.Models.DTO
{
    public class QuestionDTO
    {
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public int CreatedBy { get; set; }
        public List<QuestionOptionDTO> QuestionOptions { get; set; }
    }
}
