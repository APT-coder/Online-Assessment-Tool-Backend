using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class AssessmentScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssessmentScoreId { get; set; }
        [Required]
        [ForeignKey("ScheduledAssessment")]
        public int ScheduledAssessmentId { get; set; }
        [Required]
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public int AvergeScore { get; set; }
        public DateTime CalculatedOn { get; set; }
        public ScheduledAssessment ScheduledAssessment { get; set; }
    }
}
