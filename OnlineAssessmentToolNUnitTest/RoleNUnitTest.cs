using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework; // Ensure NUnit is imported
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAssessmentToolTestProject
{
    public class RoleNUnitTest
    {
        private Mock<IRoleRepository> _roleRepositoryMock;
        private RolesController _controller;
        private Mock<IPermissionsRepository> _permissionsRepositoryMock;
        private Mock<ILogger<RolesController>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            _loggerMock = new Mock<ILogger<RolesController>>();
            _controller = new RolesController(
             _roleRepositoryMock.Object,
             _permissionsRepositoryMock.Object,
              _loggerMock.Object);
        }

        [Test]
        public async Task GetRoles_ShouldReturnRoles()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "User" }
            };

            _roleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(roles);

            // Act
            var result = await _controller.GetRoles();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var apiResponse = okResult.Value as ApiResponse;
            Assert.IsTrue(apiResponse.IsSuccess);
            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual(roles, apiResponse.Result);
        }

        [Test]
        public async Task PostRole_ShouldReturnCreatedRole_WhenRoleIsValid()
        {
            // Arrange
            var createRoleDTO = new CreateRoleDTO
            {
                RoleName = "Admin",
                PermissionIds = new List<int> { 1, 2 }
            };

            var permissions = new List<Permission>
    {
        new Permission { Id = 1, PermissionName = "Read" },
        new Permission { Id = 2, PermissionName = "Write" }
    };

            var newRole = new Role
            {
                Id = 1,
                RoleName = "Admin",
                Permissions = permissions
            };

            _permissionsRepositoryMock
                .Setup(repo => repo.GetByIdsAsync(createRoleDTO.PermissionIds))
                .ReturnsAsync(permissions);

            _roleRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Role>()))
                .ReturnsAsync(true) // Ensure this matches the method's return type
                .Callback<Role>(role => role.Id = 1); // Simulate role creation and setting an ID

            // Act
            var result = await _controller.PostRole(createRoleDTO);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");

            var actionResult = result as ActionResult<ApiResponse>;
            Assert.IsNotNull(actionResult, "ActionResult<ApiResponse> should not be null.");
        }


    }
}
