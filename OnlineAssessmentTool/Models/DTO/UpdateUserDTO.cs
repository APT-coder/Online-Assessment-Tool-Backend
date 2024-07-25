namespace OnlineAssessmentTool.Models.DTO
{
    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public UserType UserType { get; set; }

    }
}
