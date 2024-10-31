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
        [StringLength(255)]
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastPasswordReset { get; set; }
        [Required]
        [ForeignKey("Batch")]
        public int BatchId { get; set; }
        public Batch Batch { get; set; }
    }
}

