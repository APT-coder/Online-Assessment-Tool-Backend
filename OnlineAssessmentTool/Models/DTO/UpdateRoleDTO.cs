using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Models.DTO
{
    public class UpdateRoleDTO
    {
        public string RoleName { get; set; }
        public ICollection<CreatePermissionDTO> Permissions { get; set; }
    }
}
