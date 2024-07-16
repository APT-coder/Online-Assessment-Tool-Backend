﻿using OnlineAssessmentTool.Models.DTO;
using System.Collections.Generic;

namespace OnlineAssessmentTool.Models.DTO
{
    public class CreateRoleDTO
    {
        public string RoleName { get; set; }
        public ICollection<CreatePermissionDTO> Permissions { get; set; }
    }
}