using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly APIContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(APIContext dbContext, IUserRepository userRepository, IUserService userService, ILogger<AuthController> logger)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("getUserRole/{token}")]
        public async Task<IActionResult> GetUserRoleAsync(string token)
        {
            Dictionary<string, dynamic> results = new Dictionary<string, dynamic>();
            if (string.IsNullOrEmpty(token))
            {
                return NotFound("Token not found");
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            if (tokenS != null)
            {
                var claims = tokenS.Claims;
                var upn = claims.FirstOrDefault(c => c.Type == "upn")?.Value;
                var appName = claims.FirstOrDefault(c => c.Type == "app_displayname")?.Value;

                if (upn == null || appName == null)
                {
                    return NotFound("UPN or App Display Name not found in token");
                }

                _logger.LogInformation("Fetching user details using email");
                var user = await _userService.GetUserDetailsByEmailAsync(upn);
                if (user != null)
                {
                    results.Add("appName", appName);
                    results.Add("UserId", user.UserId);
                    results.Add("UserName", user.Username);
                    results.Add("UserEmail", user.Email);
                    results.Add("UserPhone", user.Phone);
                    results.Add("UserAdmin", user.IsAdmin);
                    results.Add("UserType", user.UserType);
                    if(user.UserType == UserType.Trainer)
                    {
                        results.Add("TrainerId", user.Trainer.TrainerId);
                        results.Add("UserBatch", user.Trainer.TrainerBatch);
                        results.Add("UserRole", user.Trainer.Role);
                        results.Add("UserPermissions", user.Trainer.Role.Permissions);
                    }
                    else if(user.UserType == UserType.Trainee)
                    {
                        results.Add("TraineeId", user.Trainee.TraineeId);
                        results.Add("UserBatch", user.Trainee.Batch);
                    }

                    return Ok(results);
                }
                else
                {
                    _logger.LogWarning("User is not found");
                    return NotFound($"{upn} is not found in Employees database.");
                }
            }
            else
            {
                return BadRequest("Token cannot be converted to JwtSecurityToken");
            }
        }
    }
}
