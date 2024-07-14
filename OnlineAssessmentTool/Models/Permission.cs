using System.Data;
using System.Text.Json.Serialization;

namespace OnlineAssessmentTool.Models
{
    public class Permission
    {
        public int Id { get; set; }  // This will be auto-incremented

        public string PermissionName { get; set; }

        public string Description { get; set; }  // Description property added


        [JsonIgnore]
        public ICollection<Role> Roles { get; set; }
    }
}
