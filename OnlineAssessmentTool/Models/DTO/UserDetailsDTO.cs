namespace OnlineAssessmentTool.Models.DTO
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }
        public UserType UserType { get; set; }
        public bool IsAdmin { get; set; }
        public Trainer Trainer { get; set; }
        public Trainee Trainee { get; set; }
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; }  // Include this
    }
}
