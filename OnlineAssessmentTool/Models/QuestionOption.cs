using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class QuestionOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionOptionId { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<string> Options { get; set; }
        [Column(TypeName = "jsonb")]
        public List<string> CorrectAnswers { get; set; }
        [Required]
        public Question Question { get; set; }
    }
}
