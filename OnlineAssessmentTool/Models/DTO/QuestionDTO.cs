namespace OnlineAssessmentTool.Models.DTO
{
    public class QuestionDTO
    {
        /* public int QuestionId { get; set; }
         public int AssessmentId { get; set; }*/
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public int CreatedBy { get; set; }
        public List<QuestionOptionDTO> QuestionOptions { get; set; }
    }
}
