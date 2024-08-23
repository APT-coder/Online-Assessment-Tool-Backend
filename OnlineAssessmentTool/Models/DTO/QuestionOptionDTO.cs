using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models.DTO
{
    public class QuestionOptionDTO
    {
        [Column(TypeName = "jsonb")]
        public List<string> Options { get; set; }
        [Column(TypeName = "jsonb")]
        public List<string> CorrectAnswers { get; set; }
    }
}
