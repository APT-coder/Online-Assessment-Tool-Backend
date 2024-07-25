using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }
        [ForeignKey("Assessment")]
        public int AssessmentId { get; set; }
        [Required]
        public string QuestionType { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public int Points { get; set; }
        [Required]
        [ForeignKey("Trainer")]
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required]
        public Assessment Assessment { get; set; }
        [Required]
        public ICollection<QuestionOption> QuestionOptions { get; set; }
        public Trainer Trainer { get; set; }
        public ICollection<TraineeAnswer> TraineeAnswers { get; set; }
    }
}
