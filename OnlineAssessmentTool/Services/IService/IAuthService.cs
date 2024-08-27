using OnlineAssessmentTool.Models.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineAssessmentTool.Services.IService
{
    public interface IAuthService
    {
        JwtSecurityToken ReadJwtToken(string token);
        public Task<ApiResponse> AuthenticateUser(LoginRequestDTO loginRequest);
        public string GenerateJwtToken(UserDetailsDTO user);
    }
}
