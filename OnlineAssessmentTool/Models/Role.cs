using OnlineAssessmentTool.Models;
using System.Text.Json.Serialization;

namespace OnlineAssessmentTool.Models
{
    public class Role
    {
        public int Id { get; set; }  // This will be auto-incremented

        public string RoleName { get; set; }



        public ICollection<Permission> Permissions { get; set; }
    }
}
