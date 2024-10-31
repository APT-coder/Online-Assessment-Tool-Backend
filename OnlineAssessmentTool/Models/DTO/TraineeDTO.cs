using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models.DTO
{
    public class TraineeDTO
    {
        public DateTime JoinedOn { get; set; }
        public string? Password { get; set; }
        public int BatchId { get; set; }
    }
}
