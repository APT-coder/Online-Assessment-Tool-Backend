using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class AuthNUnitTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<AuthController>> _mockLogger;
        private Mock<IJwtService> _mockJwtService;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockJwtService = new Mock<IJwtService>();

            _authController = new AuthController(
                null,
                _mockUserRepository.Object,
                _mockUserService.Object,
                _mockLogger.Object,
                _mockJwtService.Object
            );
        }

        [Test]
        public async Task GetUserRoleAsync_ReturnsBadRequest_WhenTokenInvalid()
        {
            // Arrange
            string invalidToken = "invalidToken";
            _mockJwtService
                .Setup(s => s.ReadJwtToken(It.IsAny<string>()))
                .Throws(new SecurityTokenMalformedException("JWT is not well formed"));

            // Act
            var result = await _authController.GetUserRoleAsync(invalidToken) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Token cannot be converted to JwtSecurityToken", result.Value);
        }

        [Test]
        public async Task GetUserRoleAsync_ReturnsNotFound_WhenTokenIsNull()
        {
            // Arrange
            string nullToken = null;

            // Act
            var result = await _authController.GetUserRoleAsync(nullToken) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Token not found", result.Value);
        }

        [Test]
        public async Task GetUserRoleAsync_ReturnsNotFound_WhenUpnNotFoundInToken()
        {
            // Arrange
            string token = "valid.token.without.upn";
            var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim("some_other_claim", "value")
        };

            _mockJwtService.Setup(s => s.ReadJwtToken(It.IsAny<string>())).Returns(new JwtSecurityToken(claims: claims));

            // Act
            var result = await _authController.GetUserRoleAsync(token) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("UPN or App Display Name not found in token", result.Value);
        }

        [Test]
        public async Task GetUserRoleAsync_ReturnsOk_WhenValidToken()
        {
            // Arrange
            string token = "valid.token.with.upn";
            var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim("upn", "test@domain.com"),
            new System.Security.Claims.Claim("app_displayname", "Test App")
        };

            _mockJwtService.Setup(s => s.ReadJwtToken(It.IsAny<string>())).Returns(new JwtSecurityToken(claims: claims));

            var user = new UserDetailsDTO
            {
                UserId = 1,
                Username = "Test User",
                Email = "test@domain.com",
                Phone = "1234567890",
                IsAdmin = true,
                UserType = UserType.Trainer,
                Trainer = new Trainer
                {
                    TrainerId = 1,
                    Role = new Role
                    {
                        Permissions = new List<Permission>()
                    },
                    TrainerBatch = new List<TrainerBatch>()
                }
            };

            _mockUserService.Setup(s => s.GetUserDetailsByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authController.GetUserRoleAsync(token) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var results = result.Value as Dictionary<string, dynamic>;
            Assert.IsNotNull(results);
            Assert.AreEqual("Test App", results["appName"]);
            Assert.AreEqual(1, results["UserId"]);
            Assert.AreEqual("Test User", results["UserName"]);
        }
    }
}

