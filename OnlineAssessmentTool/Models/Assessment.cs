using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class Assessment
    {

        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
