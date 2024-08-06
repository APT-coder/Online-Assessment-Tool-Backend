using Microsoft.AspNetCore.Http;
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

        [Test]
        public async Task GetHighPerformers_ReturnsOk_WhenDataFound()
        {
            // Arrange
            var highPerformers = new List<TraineeScoreDTO>
        {
            new TraineeScoreDTO { TraineeName = "John Doe", Score = 95 }
        };

            _mockAssessmentRepository.Setup(repo => repo.GetHighPerformersByAssessmentIdAsync(1))
                .ReturnsAsync(highPerformers);

            // Act
            var result = await _controller.GetHighPerformers(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status");
            Assert.AreEqual(highPerformers, okResult.Value);
        }

        [Test]
        public async Task GetHighPerformers_ReturnsNotFound_WhenNoDataFound()
        {
            // Arrange
            _mockAssessmentRepository.Setup(repo => repo.GetHighPerformersByAssessmentIdAsync(1))
                .ReturnsAsync(new List<TraineeScoreDTO>());

            // Act
            var result = await _controller.GetHighPerformers(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status");
            Assert.IsEmpty(okResult.Value as List<TraineeScoreDTO>);
        }



        [Test]
        public async Task GetLowPerformers_ReturnsOk_WhenDataFound()
        {
            // Arrange
            var lowPerformers = new List<TraineeScoreDTO>
        {
            new TraineeScoreDTO { TraineeName = "Jane Doe", Score = 45 }
        };

            _mockAssessmentRepository.Setup(repo => repo.GetLowPerformersByAssessmentIdAsync(1))
                .ReturnsAsync(lowPerformers);

            // Act
            var result = await _controller.GetLowPerformers(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status");
            Assert.AreEqual(lowPerformers, okResult.Value);
        }

        [Test]
        public async Task GetLowPerformers_ReturnsNotFound_WhenNoDataFound()
        {
            // Arrange
            _mockAssessmentRepository.Setup(repo => repo.GetLowPerformersByAssessmentIdAsync(1))
                .ReturnsAsync(new List<TraineeScoreDTO>());

            // Act
            var result = await _controller.GetLowPerformers(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status");
            Assert.IsEmpty(okResult.Value as List<TraineeScoreDTO>);
        }

        [Test]
        public async Task GetHighPerformers_ReturnsOk_WhenEmptyListReturned()
        {
            // Arrange
            _mockAssessmentRepository.Setup(repo => repo.GetHighPerformersByAssessmentIdAsync(1))
                .ReturnsAsync(new List<TraineeScoreDTO>());

            // Act
            var result = await _controller.GetHighPerformers(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status");
            Assert.IsEmpty(okResult.Value as List<TraineeScoreDTO>, "Expected empty list but got non-empty list");
        }

        [Test]
        public async Task GetLowPerformers_ReturnsOk_WhenEmptyListReturned()
        {
            // Arrange
            _mockAssessmentRepository.Setup(repo => repo.GetLowPerformersByAssessmentIdAsync(1))
                .ReturnsAsync(new List<TraineeScoreDTO>());

            // Act
            var result = await _controller.GetLowPerformers(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status");
            Assert.IsEmpty(okResult.Value as List<TraineeScoreDTO>, "Expected empty list but got non-empty list");
        }

        [Test]
        public async Task GetTraineeAssessmentDetails_ReturnsOkResult_WithTraineeDetails()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var traineeDetails = new List<TraineeAssessmentTableDTO>
        {
            new TraineeAssessmentTableDTO { TraineeName = "John Doe", IsPresent = "Completed", Score = 85 },
            new TraineeAssessmentTableDTO { TraineeName = "Jane Smith", IsPresent = "Absent", Score = 0 }
        };

            _mockAssessmentRepository.Setup(repo => repo.GetTraineeAssessmentDetails(scheduledAssessmentId))
                                     .ReturnsAsync(traineeDetails);

            // Act
            var result = await _controller.GetTraineeAssessmentDetails(scheduledAssessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(traineeDetails, okResult.Value);
        }

        [Test]
        public async Task GetTraineeAssessmentDetails_ReturnsNotFound_WhenNoTraineeDetailsFound()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            _mockAssessmentRepository.Setup(repo => repo.GetTraineeAssessmentDetails(scheduledAssessmentId))
                                     .ReturnsAsync(new List<TraineeAssessmentTableDTO>());

            // Act
            var result = await _controller.GetTraineeAssessmentDetails(scheduledAssessmentId);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            Assert.That(notFoundResult, Is.InstanceOf<NotFoundResult>(), "Result should be of type NotFoundResult.");
        }

        [Test]
        public void Controller_IsInitialized_WithDependencies()
        {
            // Arrange & Act
            var controller = new AssessmentController(null, null, _mockAssessmentRepository.Object, null, _mockLogger.Object);

            // Assert
            Assert.NotNull(controller, "Controller should be properly initialized.");
        }

        [Test]
        public async Task GetTraineeAssessmentDetails_ReturnsCorrectDataStructure()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var traineeDetails = new List<TraineeAssessmentTableDTO>
{
    new TraineeAssessmentTableDTO { TraineeName = "John Doe", IsPresent = "Completed", Score = 85 },
    new TraineeAssessmentTableDTO { TraineeName = "Jane Smith", IsPresent = "Absent", Score = 0 }
};

            _mockAssessmentRepository.Setup(repo => repo.GetTraineeAssessmentDetails(scheduledAssessmentId))
                                     .ReturnsAsync(traineeDetails);

            // Act
            var result = await _controller.GetTraineeAssessmentDetails(scheduledAssessmentId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultData = okResult.Value as List<TraineeAssessmentTableDTO>;
            Assert.IsNotNull(resultData);
            Assert.AreEqual(2, resultData.Count, "The result should contain two trainee details.");
            Assert.AreEqual("John Doe", resultData[0].TraineeName, "The first trainee's name should be 'John Doe'.");
            Assert.AreEqual("Completed", resultData[0].IsPresent, "The first trainee's status should be 'Completed'.");
            Assert.AreEqual(85, resultData[0].Score, "The first trainee's score should be 85.");
        }

        [Test]
        public async Task GetTraineeAssessmentDetails_EndpointResponseTime_IsWithinRange()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var traineeDetails = new List<TraineeAssessmentTableDTO>
{
    new TraineeAssessmentTableDTO { TraineeName = "John Doe", IsPresent = "Completed", Score = 85 },
    new TraineeAssessmentTableDTO { TraineeName = "Jane Smith", IsPresent = "Absent", Score = 0 }
};

            _mockAssessmentRepository.Setup(repo => repo.GetTraineeAssessmentDetails(scheduledAssessmentId))
                                     .ReturnsAsync(traineeDetails);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = await _controller.GetTraineeAssessmentDetails(scheduledAssessmentId);

            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;

            // Assert
            Assert.LessOrEqual(responseTime, 500, "The response time should be within 500 milliseconds.");
        }

        [Test]
        public async Task GetTraineeAssessmentDetails_ReturnsMultipleTraineeDetails()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var traineeDetails = new List<TraineeAssessmentTableDTO>
{
    new TraineeAssessmentTableDTO { TraineeName = "John Doe", IsPresent = "Completed", Score = 85 },
    new TraineeAssessmentTableDTO { TraineeName = "Jane Smith", IsPresent = "Absent", Score = 0 },
    new TraineeAssessmentTableDTO { TraineeName = "Alice Johnson", IsPresent = "Completed", Score = 90 }
};

            _mockAssessmentRepository.Setup(repo => repo.GetTraineeAssessmentDetails(scheduledAssessmentId))
                                     .ReturnsAsync(traineeDetails);

            // Act
            var result = await _controller.GetTraineeAssessmentDetails(scheduledAssessmentId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultData = okResult.Value as List<TraineeAssessmentTableDTO>;
            Assert.IsNotNull(resultData);
            Assert.AreEqual(3, resultData.Count, "The result should contain three trainee details.");
        }

    }
}

