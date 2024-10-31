using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;

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
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AuthController(APIContext dbContext, IUserRepository userRepository, IUserService userService, ILogger<AuthController> logger, IAuthService authService, IEmailService emailService, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userService = userService;
            _logger = logger;
            _authService = authService;
            _emailService = emailService;
            _cache = cache;
        }

        [HttpGet("getUserRole/{token}")]
        public async Task<IActionResult> AzureSSOLogin(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return NotFound("Token not found");
            }

            try
            {
                var loginResponse = await _authService.AuthenticateSSOUser(token);
                return Ok(loginResponse);
            }
            catch (SecurityTokenMalformedException)
            {
                _logger.LogError("Invalid token format");
                return BadRequest("Token cannot be converted to JwtSecurityToken");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExternalTrainerLogin([FromBody] LoginRequestDTO loginRequest)
        {
            var loginResponse = await _authService.AuthenticateUser(loginRequest);
            return Ok(loginResponse);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] OtpRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email address is required.");
            }

            var otp = _authService.GenerateOtp();
            _cache.Set(request.Email, otp, TimeSpan.FromMinutes(5));

            var emailResponse = await _emailService.SendEmailAsync(request.Email, "OTP for Password Reset - Team Knowlix", $"Your OTP code is: {otp}");

            if (emailResponse.Successful)
            {
                return Ok(new { message = "OTP sent successfully." });
            }
            else
            {
                return StatusCode(500, "Failed to send OTP.");
            }
        }

        [HttpPost("verify")]
        public IActionResult VerifyOtp([FromBody] OtpVerificationRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
            {
                return BadRequest("Email and OTP are required.");
            }

            if (_cache.TryGetValue(request.Email, out string storedOtp))
            {
                if (storedOtp == request.Otp)
                {
                    return Ok(new { message = "OTP verified successfully." });
                }
                else
                {
                    return BadRequest("Invalid OTP.");
                }
            }
            else
            {
                return BadRequest("OTP expired or not found.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> TrainerResetPassword([FromBody] UpdateTrainerPasswordDTO updateTrainerPasswordDTO)
        {
            var resetPasswordResponse = await _userService.UpdateTrainerPasswordAsync(updateTrainerPasswordDTO);
            return Ok(resetPasswordResponse);
        }
    }
}
