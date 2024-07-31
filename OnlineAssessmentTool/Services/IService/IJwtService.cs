using System.IdentityModel.Tokens.Jwt;

namespace OnlineAssessmentTool.Services.IService
{
    public interface IJwtService
    {
        JwtSecurityToken ReadJwtToken(string token);
    }
}
