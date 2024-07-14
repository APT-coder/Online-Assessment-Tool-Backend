using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class Batch
    {
        [Key]
        public int batchid { get; set; }

        [Required]
        [StringLength(100)]
        public string batchname { get; set; }
    }
}
