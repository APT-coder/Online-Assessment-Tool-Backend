using OnlineAssessmentTool.Services.IService;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineAssessmentTool.Services
{
    public class JwtService : IJwtService
    {
        public JwtSecurityToken ReadJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }
    }
}
