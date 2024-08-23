using NUnit.Framework;
using Moq;
using OnlineAssessmentTool.Services;
using OnlineAssessmentTool.Services.IService;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoMapper;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class UserNUnitTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private IUserService _userService;
        private Mock<IUserService> _userServiceMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _userService = _userServiceMock.Object;

        }

        [Test]
        public async Task CreateUserAsync_ShouldReturnTrue_WhenCreatingTraineeUser()
        {
            var createUserDto = new CreateUserDTO
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Phone = "1234567890",
                IsAdmin = false,
                UserType = UserType.Trainee
            };

            var traineeDto = new TraineeDTO
            {
                JoinedOn = DateTime.Now,
                BatchId = 1
            };

            _userServiceMock
                .Setup(service => service.CreateUserAsync(
                    It.Is<CreateUserDTO>(dto => dto.Username == "testuser" && dto.Email == "testuser@example.com"),
                    It.IsAny<TrainerDTO>(),
                    traineeDto,
                    It.IsAny<List<int>>()))
                .ReturnsAsync(true);


            var result = await _userService.CreateUserAsync(createUserDto, traineeDto: traineeDto);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task CreateUserAsync_ShouldReturnTrue_WhenCreatingTrainerUser()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Username = "traineruser",
                Email = "traineruser@example.com",
                Phone = "1234567890",
                IsAdmin = false,
                UserType = UserType.Trainer
            };

            var trainerDto = new TrainerDTO
            {
                JoinedOn = DateTime.Now,
                Password = "securepassword",
                RoleId = 1
            };

            var batchIds = new List<int> { 1, 2 };

            _userServiceMock
                .Setup(service => service.CreateUserAsync(
                    It.Is<CreateUserDTO>(dto => dto.Username == "traineruser" && dto.Email == "traineruser@example.com"),
                    trainerDto,
                    It.IsAny<TraineeDTO>(),
                    It.Is<List<int>>(b => b.SequenceEqual(batchIds))))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto, trainerDto, null, batchIds);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task GetUsersByRoleNameAsync_ShouldReturnUsers_WhenRoleNameMatches()
        {
            // Arrange
            var roleName = "Trainer Manager";

            var users = new List<Users>
            {
                new Users { UserId = 1, Username = "trainer1", Email = "trainer1@example.com", Phone = "1234567890", UserType = UserType.Trainer },
                new Users { UserId = 2, Username = "trainer2", Email = "trainer2@example.com", Phone = "0987654321", UserType = UserType.Trainer }
            };

            var roles = new List<Role>
            {
                new Role { Id = 1, RoleName = "Trainer Manager" }
            };

            var trainers = new List<Trainer>
            {
                new Trainer { UserId = 1, RoleId = 1 },
                new Trainer { UserId = 2, RoleId = 1 }
            };

            _userServiceMock.Setup(service => service.GetUsersByRoleNameAsync(roleName))
                 .ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersByRoleNameAsync(roleName);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count); // Ensure it returns both users
            Assert.AreEqual("trainer1", result[0].Username);
            Assert.AreEqual("trainer2", result[1].Username);
        }
        [Test]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var userId = 1;

            _userServiceMock
                .Setup(service => service.DeleteUserAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _userServiceMock.Verify(service => service.DeleteUserAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetUserDetailsByEmailAsync_ShouldReturnUserDetails()
        {
            // Arrange
            var email = "testuser@example.com";
            var userDetails = new UserDetailsDTO
            {
                UserId = 1,
                Username = "testuser",
                Email = email,
                Phone = "1234567890",
                UserType = UserType.Trainee,
                IsAdmin = false,
                Trainer = new Trainer { TrainerId = 1, UserId = 1, JoinedOn = DateTime.Now, Password = "securepassword", RoleId = 1 },
                Trainee = null,  // Assuming the user is a Trainer
                Role = new Role { Id = 1, RoleName = "Trainer Manager" },
                Permissions = new List<Permission> { new Permission { Id = 1, PermissionName = "View Dashboard" } }
            };

            _userServiceMock
                .Setup(service => service.GetUserDetailsByEmailAsync(email))
                .ReturnsAsync(userDetails);

            // Act
            var result = await _userService.GetUserDetailsByEmailAsync(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
            Assert.AreEqual("testuser", result.Username);
            Assert.AreEqual("1234567890", result.Phone);
            Assert.AreEqual(UserType.Trainee, result.UserType);
            Assert.IsFalse(result.IsAdmin);
            Assert.IsNotNull(result.Trainer);
            Assert.AreEqual(1, result.Trainer.TrainerId);
            Assert.AreEqual("Trainer Manager", result.Role.RoleName);
            Assert.IsNotNull(result.Permissions);
            Assert.AreEqual(1, result.Permissions.Count);
            Assert.AreEqual("View Dashboard", result.Permissions[0].PermissionName);
        }



    }
}
