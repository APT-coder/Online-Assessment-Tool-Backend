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

        public int CreatedBy { get; set; }
        [Required]
        public ICollection<Question> Questions { get; set; }
    }
}
