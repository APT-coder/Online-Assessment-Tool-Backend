using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Models
{
    public class Role
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // This will be auto-incremented

        public string RoleName { get; set; }



        public ICollection<Permission> Permissions { get; set; }
    }
}
