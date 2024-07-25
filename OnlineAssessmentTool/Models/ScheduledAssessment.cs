using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class ScheduledAssessment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduledAssessmentId { get; set; }
        [Required]
        public int BatchId { get; set; }
        [Required]
        public int AssessmentId { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }
        [Required]
        public TimeSpan AssessmentDuration { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public AssessmentStatus Status { get; set; }
        public bool CanRandomizeQuestion { get; set; }
        public bool CanDisplayResult { get; set; }
        public bool CanSubmitBeforeEnd { get; set; }
    }
}
