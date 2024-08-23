using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAssessmentToolNUnitTest
{
    public class TraineeAnswerNUnitTest
    {
        [TestFixture]
        public class TraineeAnswerControllerNUnitTest
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
            public async Task UpdateScore_ReturnsNotFound_WhenTraineeAnswerIsNotFound()
            {
                // Arrange
                var updateScoreDTO = new UpdateScoreDTO
                {
                    ScheduledAssessmentId = 1,
                    TraineeId = 1,
                    QuestionId = 1,
                    Score = 100
                };

                _mockTraineeAnswerRepository.Setup(repo => repo.GetTraineeAnswerAsync(
                    updateScoreDTO.ScheduledAssessmentId,
                    updateScoreDTO.TraineeId,
                    updateScoreDTO.QuestionId)).ReturnsAsync((TraineeAnswer)null);

                // Act
                var result = await _controller.UpdateScore(updateScoreDTO) as ActionResult;

                // Assert
                Assert.IsNotNull(result);

                var notFoundResult = result as NotFoundObjectResult;
                Assert.IsNotNull(notFoundResult);
                //Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            }

            [Test]
            public async Task UpdateScore_ReturnsOk_WhenScoreIsUpdatedSuccessfully()
            {
                // Arrange
                var updateScoreDTO = new UpdateScoreDTO
                {
                    ScheduledAssessmentId = 1,
                    TraineeId = 1,
                    QuestionId = 1,
                    Score = 100
                };

                var traineeAnswer = new TraineeAnswer
                {
                    ScheduledAssessmentId = 1,
                    TraineeId = 1,
                    QuestionId = 1,
                    Score = 90
                };

                _mockTraineeAnswerRepository.Setup(repo => repo.GetTraineeAnswerAsync(
                    updateScoreDTO.ScheduledAssessmentId,
                    updateScoreDTO.TraineeId,
                    updateScoreDTO.QuestionId)).ReturnsAsync(traineeAnswer);

                _mockTraineeAnswerRepository.Setup(repo => repo.UpdateTraineeAnswerAsync(It.IsAny<TraineeAnswer>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.UpdateScore(updateScoreDTO) as ActionResult;

                // Assert
                Assert.IsNotNull(result);

                var okResult = result as OkObjectResult;
                Assert.IsNotNull(okResult);
                // Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.AreEqual(traineeAnswer, okResult.Value);
            }

            [Test]
            public async Task UpdateScore_ReturnsOkWithUpdatedScore()
            {
                // Arrange
                var updateScoreDTO = new UpdateScoreDTO
                {
                    ScheduledAssessmentId = 1,
                    TraineeId = 1,
                    QuestionId = 1,
                    Score = 95
                };

                var traineeAnswer = new TraineeAnswer
                {
                    ScheduledAssessmentId = 1,
                    TraineeId = 1,
                    QuestionId = 1,
                    Score = 80
                };

                _mockTraineeAnswerRepository.Setup(repo => repo.GetTraineeAnswerAsync(
                    updateScoreDTO.ScheduledAssessmentId,
                    updateScoreDTO.TraineeId,
                    updateScoreDTO.QuestionId)).ReturnsAsync(traineeAnswer);

                _mockTraineeAnswerRepository.Setup(repo => repo.UpdateTraineeAnswerAsync(It.IsAny<TraineeAnswer>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.UpdateScore(updateScoreDTO) as ActionResult;

                // Assert
                Assert.IsNotNull(result);
                var okResult = result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.AreEqual(updateScoreDTO.Score, ((TraineeAnswer)okResult.Value).Score);
            }
            [Test]
            public async Task UpdateScore_ReturnsNotFound_WhenUpdateFails()
            {
                // Arrange
                var updateScoreDTO = new UpdateScoreDTO
                {
                    ScheduledAssessmentId = 1,
                    TraineeId = 1,
                    QuestionId = 1,
                    Score = 100
                };

                _mockTraineeAnswerRepository.Setup(repo => repo.GetTraineeAnswerAsync(
                    updateScoreDTO.ScheduledAssessmentId,
                    updateScoreDTO.TraineeId,
                    updateScoreDTO.QuestionId)).ReturnsAsync((TraineeAnswer)null);

                // Act
                var result = await _controller.UpdateScore(updateScoreDTO) as ActionResult;

                // Assert
                Assert.IsNotNull(result);
                var notFoundResult = result as NotFoundObjectResult;
                Assert.IsNotNull(notFoundResult);
                Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            }
        }


    }
}