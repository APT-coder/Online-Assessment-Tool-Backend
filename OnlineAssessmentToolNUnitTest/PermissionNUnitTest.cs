using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class PermissionNUnitTest
    {
        private Mock<IPermissionsRepository> _permissionRepositoryMock;
        private Mock<ILogger<PermissionController>> _loggerMock;
        private PermissionController _controller;

        [SetUp]
        public void SetUp()
        {
            _permissionRepositoryMock = new Mock<IPermissionsRepository>();
            _loggerMock = new Mock<ILogger<PermissionController>>();
            _controller = new PermissionController(_permissionRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllPermissions_ReturnsOkResult_WithListOfPermissions()
        {
            // Arrange
            var permissions = new List<Permission> { new Permission { Id = 1, PermissionName = "TestPermission", Description = "TestDescription" } };
            _permissionRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(permissions);

            // Act
            var result = await _controller.GetAllPermissions();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var response = okResult.Value as ApiResponse;
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(permissions, response.Result);
        }

        [Test]
        public async Task GetPermission_ReturnsOkResult_WithPermission()
        {
            // Arrange
            var permission = new Permission { Id = 1, PermissionName = "TestPermission", Description = "TestDescription" };
            _permissionRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(permission);

            // Act
            var result = await _controller.GetPermission(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var response = okResult.Value as ApiResponse;
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(permission, response.Result);
        }

        [Test]
        public async Task GetPermission_ReturnsNotFound_WhenPermissionDoesNotExist()
        {
            // Arrange
            _permissionRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Permission)null);

            // Act
            var result = await _controller.GetPermission(1);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);

            var response = notFoundResult.Value as ApiResponse;
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("Permission not found", response.Message[0]);
        }

        [Test]
        public async Task CreatePermission_ReturnsCreatedAtAction_WithPermission()
        {
            // Arrange
            var createDto = new CreatePermissionDTO { PermissionName = "TestPermission", Description = "TestDescription" };
            var permission = new Permission { Id = 1, PermissionName = createDto.PermissionName, Description = createDto.Description };

            _permissionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Permission>()))
                .ReturnsAsync(true)
                .Callback<Permission>(p => p.Id = permission.Id);

            // Act
            var result = await _controller.CreatePermission(createDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual((int)HttpStatusCode.Created, createdAtActionResult.StatusCode);

            var response = createdAtActionResult.Value as ApiResponse;
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Result);
            var createdPermission = response.Result as Permission;
            Assert.AreEqual(permission.Id, createdPermission.Id);
            Assert.AreEqual(permission.PermissionName, createdPermission.PermissionName);
            Assert.AreEqual(permission.Description, createdPermission.Description);
        }



        [Test]
        public async Task UpdatePermission_ReturnsOkResult_WithUpdatedPermission()
        {
            // Arrange
            var updateDto = new UpdatePermissionDTO { Id = 1, PermissionName = "UpdatedPermission", Description = "UpdatedDescription" };
            var existingPermission = new Permission { Id = 1, PermissionName = "TestPermission", Description = "TestDescription" };

            _permissionRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPermission);
            _permissionRepositoryMock.Setup(repo => repo.UpdateAsync(existingPermission)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdatePermission(1, updateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var response = okResult.Value as ApiResponse;
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(updateDto.PermissionName, (response.Result as Permission).PermissionName);
        }

        [Test]
        public async Task DeletePermission_ReturnsOkResult_WhenPermissionDeleted()
        {
            // Arrange
            var permissionId = 1;
            var permission = new Permission { Id = permissionId, PermissionName = "TestPermission", Description = "TestDescription" };

            _permissionRepositoryMock.Setup(repo => repo.GetByIdAsync(permissionId)).ReturnsAsync(permission);
            _permissionRepositoryMock.Setup(repo => repo.DeleteAsync(permission)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePermission(permissionId);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okObjectResult.StatusCode);

            var response = okObjectResult.Value as ApiResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNull(response.Result);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

    }
}
