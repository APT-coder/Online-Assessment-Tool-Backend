namespace OnlineAssessmentTool.Models.DTO
{
    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public UserType UserType { get; set; } // Enum for selecting user type (Trainer or Trainee)
        public Guid UUID { get; set; }
    }
}
