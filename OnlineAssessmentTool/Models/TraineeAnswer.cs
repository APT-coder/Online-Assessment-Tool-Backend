using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class TraineeAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TraineeAnswerId { get; set; }

        [Required]
        [ForeignKey("ScheduledAssessment")]
        public int ScheduledAssessmentId { get; set; }

        [Required]
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }

        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
        /* public ScheduledAssessment ScheduledAssessment { get; set; }
         public Users Users { get; set; }*/
        public Question Question { get; set; }


    }
}
