using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class AssessmentScoreControllerTests
    {
        private Mock<IAssessmentScoreRepository> _mockRepository;
        private Mock<IAssessmentScoreService> _mockService;
        private Mock<ILogger<AssessmentScoreController>> _mockLogger;
        private AssessmentScoreController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IAssessmentScoreRepository>();
            _mockService = new Mock<IAssessmentScoreService>();
            _mockLogger = new Mock<ILogger<AssessmentScoreController>>();
            _controller = new AssessmentScoreController(_mockRepository.Object, _mockService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetScoreDistribution_ReturnsOkResult_WithScoreDistribution()
        {
            // Arrange
            int assessmentId = 1;
            var scoreDistribution = new List<object>
            {
                new { Category = "Above 90%", Count = 5 },
                new { Category = "80% - 90%", Count = 10 }
            };

            _mockRepository.Setup(repo => repo.GetScoreDistributionAsync(assessmentId)).ReturnsAsync(scoreDistribution);

            // Act
            var result = await _controller.GetScoreDistribution(assessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(scoreDistribution, okResult.Value);
        }

        [Test]
        public async Task GetScoreDistribution_ReturnsOkResult_WhenAssessmentIdIsInvalid()
        {
            // Arrange
            int invalidAssessmentId = -1;
            var scoreDistribution = new List<object>(); // Assuming invalid ID leads to no data

            _mockRepository.Setup(repo => repo.GetScoreDistributionAsync(invalidAssessmentId)).ReturnsAsync(scoreDistribution);

            // Act
            var result = await _controller.GetScoreDistribution(invalidAssessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(scoreDistribution, okResult.Value);
        }





    }
}