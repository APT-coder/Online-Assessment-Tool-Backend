using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace OnlineAssessmentTool.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _service;
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository repository, IUserService service, IConfiguration configuration)
        {
            _repository = repository;
            _service = service;
            _configuration = configuration;
        }
        public JwtSecurityToken ReadJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public async Task<ApiResponse> AuthenticateUser(LoginRequestDTO loginRequest)
        {
            var result = new LoginResponseDTO();
            var response = new ApiResponse();

            if (!await _service.ValidateUserAsync(loginRequest.Email, loginRequest.Password))
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }
            else if(!await _service.IsUserActive(loginRequest.Email))
            {
                response.IsSuccess = false;
                response.Result = loginRequest.Email;
                response.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }

            var user = await _service.GetUserDetailsByEmailAsync(loginRequest.Email);

            var token = GenerateJwtToken(user);

            result.UserDetails = user;
            result.Token = token;

            response.Result = result;
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return response;
        }

        public string GenerateJwtToken(UserDetailsDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(ClaimTypes.Upn, user.Email.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWTSecretKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
