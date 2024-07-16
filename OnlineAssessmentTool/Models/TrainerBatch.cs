using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class TrainerBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerBatchId { get; set; }

        [Required]
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }

        [Required]
        [ForeignKey("Batch")]
        public int BatchId { get; set; }

        public Trainer Trainer { get; set; }
        public Batch Batch { get; set; }
    }
}
