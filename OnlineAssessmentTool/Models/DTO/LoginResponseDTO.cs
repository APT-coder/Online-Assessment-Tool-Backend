namespace OnlineAssessmentTool.Models.DTO
{
    public class LoginResponseDTO
    {
        public UserDetailsDTO UserDetails { get; set; }
        public string Token { get; set; }
    }
}
