﻿namespace OnlineAssessmentTool.Models.DTO
{
    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public int[] BatchIds { get; set; }
        public int RoleId { get; set; }
    }
}