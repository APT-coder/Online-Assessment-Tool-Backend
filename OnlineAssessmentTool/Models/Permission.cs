using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineAssessmentTool.Models
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Role> Roles { get; set; }
    }
}
