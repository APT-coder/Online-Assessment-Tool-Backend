using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class Assessment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssessmentId { get; set; }
        [Required]
        public string AssessmentName { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required]
        [ForeignKey("Trainer")]
        public int CreatedBy { get; set; }
        public int? TotalScore { get; set; }
        [Required]
        public ICollection<Question> Questions { get; set; }
        public Trainer Trainer { get; set; }
    }
}
