using OnlineAssessmentTool.Models.DTO;
using System;
using System.Collections.Generic;
namespace OnlineAssessmentTool.Models.DTO
{
    public class PostAssessmentDTO
    {
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<QuestionOptionDTO> QuestionOptions { get; set; }
        public int SelectedOption { get; set; }
        public string Answered { get; set; }
        public string QuestionStatus { get; set; }
        public int QuestionNo { get; set; }
    }

}


