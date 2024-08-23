using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;

namespace OnlineAssessmentTool.Tests.Controllers
{
    [TestFixture]
    public class TraineeAnswerControllerTests
    {
        private Mock<ITraineeAnswerRepository> _mockTraineeAnswerRepository;
        private Mock<IAssessmentPostService> _mockAssessmentPostService;
        private Mock<ILogger<TraineeAnswerController>> _mockLogger;
        private TraineeAnswerController _controller;

        [SetUp]
        public void Setup()
        {
            _mockTraineeAnswerRepository = new Mock<ITraineeAnswerRepository>();
            _mockAssessmentPostService = new Mock<IAssessmentPostService>();
            _mockLogger = new Mock<ILogger<TraineeAnswerController>>();
            _controller = new TraineeAnswerController(
                _mockTraineeAnswerRepository.Object,
                _mockAssessmentPostService.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task AssessmentSubmit_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var questions = new List<PostAssessmentDTO>
            {
                new PostAssessmentDTO { /* Initialize with test data */ }
            };
            int userId = 1;

            _mockAssessmentPostService
                .Setup(s => s.ProcessTraineeAnswers(It.IsAny<List<PostAssessmentDTO>>(), It.IsAny<int>()))
                .ReturnsAsync(new List<TraineeAnswer>()); // Return an empty list of TraineeAnswer

            // Act
            var result = await _controller.AssessmentSubmit(questions, userId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Assessment submitted successfully")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Test]
        public async Task AssessmentSubmit_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var questions = new List<PostAssessmentDTO>
            {
                new PostAssessmentDTO { /* Initialize with test data */ }
            };
            int userId = 1;

            _mockAssessmentPostService
                .Setup(s => s.ProcessTraineeAnswers(It.IsAny<List<PostAssessmentDTO>>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.AssessmentSubmit(questions, userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var response = objectResult.Value as ApiResponse;
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("Error processing the assessment.", response.Message[0]);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("An error occurred while submitting the assessment")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Test]
        public async Task AssessmentSubmit_EmptyQuestionsList_ReturnsBadRequest()
        {
            // Arrange
            var questions = new List<PostAssessmentDTO>();
            int userId = 1;

            // Act
            var result = await _controller.AssessmentSubmit(questions, userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("No questions provided for assessment.", badRequestResult.Value);
        }

        [Test]
        public async Task AssessmentSubmit_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var questions = new List<PostAssessmentDTO>
            {
                new PostAssessmentDTO { /* Initialize with test data */ }
            };
            int userId = -1; // Invalid user ID

            // Act
            var result = await _controller.AssessmentSubmit(questions, userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Invalid user ID.", badRequestResult.Value);
        }

        [Test]
        public async Task AssessmentSubmit_NullQuestionsList_ReturnsBadRequest()
        {
            // Arrange
            List<PostAssessmentDTO> questions = null;
            int userId = 1;

            // Act
            var result = await _controller.AssessmentSubmit(questions, userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Questions list cannot be null.", badRequestResult.Value);
        }

        [Test]
        public async Task AssessmentSubmit_ProcessTraineeAnswersReturnsEmptyList_ReturnsOk()
        {
            // Arrange
            var questions = new List<PostAssessmentDTO>
            {
                new PostAssessmentDTO { /* Initialize with test data */ }
            };
            int userId = 1;

            _mockAssessmentPostService
                .Setup(s => s.ProcessTraineeAnswers(It.IsAny<List<PostAssessmentDTO>>(), It.IsAny<int>()))
                .ReturnsAsync(new List<TraineeAnswer>());

            // Act
            var result = await _controller.AssessmentSubmit(questions, userId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}