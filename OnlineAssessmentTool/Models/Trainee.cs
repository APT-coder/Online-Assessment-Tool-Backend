using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class Trainee
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TraineeId { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users User { get; set; }

        [Required]
        public DateTime JoinedOn { get; set; }

        [Required]
        [ForeignKey("Batch")]
        public int BatchId { get; set; }

        // Navigation property for Batch
        /* public Batch Batch { get; set; }
         public AssessmentScore AssessmentScore { get; set; }*/
    }
}

