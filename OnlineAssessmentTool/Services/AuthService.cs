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

        public async Task<ApiResponse> AuthenticateSSOUser(string token)
        {
            var response = new ApiResponse();
            Dictionary<string, dynamic> results = new Dictionary<string, dynamic>();

            var tokenS = ReadJwtToken(token);
            var claims = tokenS.Claims;
            var upn = claims.FirstOrDefault(c => c.Type == "upn")?.Value;
            var appName = claims.FirstOrDefault(c => c.Type == "app_displayname")?.Value;

            if (upn == null || appName == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = ["UPN or App Display Name not found in token"];
                return response;
            }
            var user = await _service.GetUserDetailsByEmailAsync(upn);
            if (user == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Message = ["User not found"];
                return response;
            }
            else
            {
                var tokenNew = GenerateJwtToken(user);
                results.Add("appName", appName);
                results.Add("Token", tokenNew);
                results.Add("UserId", user.UserId);
                results.Add("UserName", user.Username);
                results.Add("UserEmail", user.Email);
                results.Add("UserPhone", user.Phone);
                results.Add("UserAdmin", user.IsAdmin);
                results.Add("UserType", user.UserType);
                if (user.UserType == UserType.Trainer)
                {
                    results.Add("TrainerId", user.Trainer.TrainerId);
                    results.Add("UserBatch", user.Trainer.TrainerBatch);
                    results.Add("UserRole", user.Trainer.Role);
                    results.Add("UserPermissions", user.Trainer.Role.Permissions);
                }
                else if (user.UserType == UserType.Trainee)
                {
                    results.Add("TraineeId", user.Trainee.TraineeId);
                    results.Add("UserBatch", user.Trainee.Batch);
                }

                response.Result = results;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                return response;
            }
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
            else if (!await _service.IsUserActive(loginRequest.Email))
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
            string roleObject = string.Empty;

            if (user.IsAdmin)
            {
                roleObject = "Admin";
            }
            else if (user.UserType == UserType.Trainer)
            {
                roleObject = "Trainer";
            }
            else
            {
                roleObject = "Trainee";
            }

            var claims = new[]
            {
                new Claim("upn", user.Email.ToString()),
                new Claim("app_displayname", "Knowlix"),
                new Claim(ClaimTypes.Role, roleObject)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("HelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorld"));
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

        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
