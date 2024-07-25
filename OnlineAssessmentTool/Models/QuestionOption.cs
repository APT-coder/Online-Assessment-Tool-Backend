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
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        [Required]
        public string CorrectAnswer { get; set; }
        [Required]
        public Question Question { get; set; }
    }
}
