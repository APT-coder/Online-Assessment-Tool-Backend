using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class AssessmentNUnitTest
    {
        private Mock<IAssessmentService> _mockAssessmentService;
        private Mock<IQuestionService> _mockQuestionService;
        private Mock<IAssessmentRepository> _mockAssessmentRepository;
        private Mock<IQuestionRepository> _mockQuestionRepository;
        private AssessmentController _controller;
        private Mock<ILogger<AssessmentController>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            
            _mockAssessmentService = new Mock<IAssessmentService>();
            _mockQuestionService = new Mock<IQuestionService>();
            _mockAssessmentRepository = new Mock<IAssessmentRepository>();
            _mockQuestionRepository = new Mock<IQuestionRepository>();
            _mockLogger = new Mock<ILogger<AssessmentController>>();


            _controller = new AssessmentController(
                _mockAssessmentService.Object,
                _mockQuestionService.Object,
                _mockAssessmentRepository.Object,
                _mockQuestionRepository.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task GetAllAssessments_ShouldReturnSuccessResponse()
        {
            // Arrange
            var assessments = new List<Assessment>
            {
                new Assessment {AssessmentId = 1, AssessmentName = "Assessment 1",CreatedOn=DateTime.UtcNow,CreatedBy=1 },
                new Assessment {AssessmentId = 2, AssessmentName = "Assessment 2",CreatedOn=DateTime.UtcNow,CreatedBy=1 }
            };

            // Set up the mock repository to return the list of assessments
            _mockAssessmentRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(assessments);

            // Act
            var result = await _controller.GetAllAssessments();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            var apiResponse = okResult.Value as ApiResponse;

            Assert.IsTrue(apiResponse.IsSuccess);
            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual(assessments, apiResponse.Result);
            Assert.Contains("Assessments retrieved successfully.", apiResponse.Message);
        }

        [Test]
        public async Task CreateAssessment_ReturnsCreatedAtActionResult_WithApiResponse()
        {
            // Arrange
            var assessmentDTO = new AssessmentDTO
            {
                AssessmentName = "New Assessment",
                CreatedBy = 1
            };

            var assessment = new Assessment
            {
                AssessmentId = 1,
                AssessmentName = "New Assessment",
                CreatedOn = DateTime.UtcNow,
                CreatedBy = 1,
                 Questions = new List<Question>()
            };

           
            _mockAssessmentService.Setup(service => service.CreateAssessmentAsync(assessmentDTO))
                                  .ReturnsAsync(assessment);

            // Act
            var result = await _controller.CreateAssessment(assessmentDTO) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);

            var response = result.Value as ApiResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("Assessment created successfully.", response.Message.First());

            var resultAssessment = response.Result as Assessment;
            Assert.IsNotNull(resultAssessment);
            Assert.AreEqual(1, resultAssessment.AssessmentId);
            Assert.AreEqual("New Assessment", resultAssessment.AssessmentName);
            Assert.AreEqual(1, resultAssessment.CreatedBy);
        }

        [Test]
        public async Task AddQuestionToAssessment_ReturnsCreatedAtActionResult_WithApiResponse()
        {
            // Arrange
            int assessmentId = 1;
            var questionDTO = new QuestionDTO
            {
                QuestionType = "MCQ",
                QuestionText = "What is the capital of France?",
                Points = 10,
                CreatedBy = 1,
                QuestionOptions = new List<QuestionOptionDTO>
                {
                    new QuestionOptionDTO
                    {
                        Option1 = "Paris",
                        Option2 = "London",
                        Option3 = "Berlin",
                        Option4 = "Madrid",
                        CorrectAnswer = "Paris"
                    }
                }
            };

            var question = new Question
            {
                QuestionId = 1,
                AssessmentId = assessmentId,
                QuestionType = "MCQ",
                QuestionText = "What is the capital of France?",
                Points = 10,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow,
                QuestionOptions = new List<QuestionOption>
                {
                    new QuestionOption
                    {
                        Option1 = "Paris",
                        Option2 = "London",
                        Option3 = "Berlin",
                        Option4 = "Madrid",
                        CorrectAnswer = "Paris"
                    }
                }
            };

            _mockQuestionService.Setup(service => service.AddQuestionToAssessmentAsync(assessmentId, questionDTO))
                                .ReturnsAsync(question);

            // Act
            var result = await _controller.AddQuestionToAssessment(assessmentId, questionDTO) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);

            var response = result.Value as ApiResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("Question added successfully.", response.Message.First());

            var resultQuestion = response.Result as Question;
            Assert.IsNotNull(resultQuestion);
            Assert.AreEqual(1, resultQuestion.QuestionId);
            Assert.AreEqual(assessmentId, resultQuestion.AssessmentId);
            Assert.AreEqual("MCQ", resultQuestion.QuestionType);
            Assert.AreEqual("What is the capital of France?", resultQuestion.QuestionText);
            Assert.AreEqual(10, resultQuestion.Points);
            Assert.AreEqual(1, resultQuestion.CreatedBy);
            Assert.IsNotNull(resultQuestion.QuestionOptions);
            Assert.AreEqual(1, resultQuestion.QuestionOptions.Count);
            Assert.AreEqual("Paris", resultQuestion.QuestionOptions.First().Option1);
        }

        [Test]
        public async Task AddQuestionToAssessment_ReturnsNotFound_WhenAssessmentNotFound()
        {
            // Arrange
            int assessmentId = 1;
            var questionDTO = new QuestionDTO
            {
                QuestionType = "MCQ",
                QuestionText = "What is the capital of France?",
                Points = 10,
                CreatedBy = 1,
                QuestionOptions = new List<QuestionOptionDTO>
                {
                    new QuestionOptionDTO
                    {
                        Option1 = "Paris",
                        Option2 = "London",
                        Option3 = "Berlin",
                        Option4 = "Madrid",
                        CorrectAnswer = "Paris"
                    }
                }
            };

            
            _mockQuestionService.Setup(service => service.AddQuestionToAssessmentAsync(assessmentId, questionDTO))
                                .ReturnsAsync((Question)null);

            // Act
            var result = await _controller.AddQuestionToAssessment(assessmentId, questionDTO) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

            var response = result.Value as ApiResponse;
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("Assessment not found.", response.Message.First());
        }

        [Test]
        public async Task UpdateAssessmentTotalScore_ExistingAssessment_ReturnsOkResult()
        {
            // Arrange
            int assessmentId = 1;
            var updateDto = new UpdateAssessmentTotalScoreDTO { TotalScore = 95 };
            var existingAssessment = new Assessment
            {
                AssessmentId = assessmentId,
                AssessmentName = "Test Assessment",
                CreatedBy = 1,
                TotalScore = 90,
                Questions = new List<Question>()
            };

            _mockAssessmentRepository
                .Setup(repo => repo.GetAssessmentByIdAsync(assessmentId))
                .ReturnsAsync(existingAssessment);

            // Act
            var result = await _controller.UpdateAssessmentTotalScore(assessmentId, updateDto) as OkObjectResult;
            var response = result.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(updateDto.TotalScore, existingAssessment.TotalScore);
            Assert.AreEqual("Assessment updated successfully.", response.Message[0]);
        }

        [Test]
        public async Task UpdateAssessmentTotalScore_AssessmentNotFound_ReturnsNotFound()
        {
            // Arrange
            int assessmentId = 1;
            var updateDto = new UpdateAssessmentTotalScoreDTO { TotalScore = 95 };

            // Mock the repository to return null for the assessment
            _mockAssessmentRepository
                .Setup(repo => repo.GetAssessmentByIdAsync(assessmentId))
                .ReturnsAsync((Assessment)null);

            // Act
            var result = await _controller.UpdateAssessmentTotalScore(assessmentId, updateDto) as NotFoundObjectResult;
            var response = result?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode, "Status code should be 404 Not Found");

            // Check if the response is not null
            Assert.IsNotNull(response, "Response should not be null");

            // Check the success flag
            Assert.IsFalse(response.IsSuccess, "Response should indicate failure");

            // Check if the Message list is not empty and contains the expected error message
            Assert.IsNotNull(response.Message, "Message list should not be null");
            Assert.IsNotEmpty(response.Message, "Message list should contain at least one item");
            Assert.AreEqual("Assessment not found.", response.Message.First(), "Message should match expected error message");
        }

    }
}

