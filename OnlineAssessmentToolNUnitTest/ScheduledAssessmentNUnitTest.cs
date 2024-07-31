using Moq;
using NUnit.Framework;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class ScheduledAssessmentNUnitTest
    {
        private Mock<IScheduledAssessmentRepository> _mockScheduledAssessmentRepository;
        private Mock<IScheduledAssessmentService> _mockScheduledAssessmentService;
        private Mock<ILogger<ScheduledAssessmentController>> _mockLogger;
        private ScheduledAssessmentController _controller;

        [SetUp]
        public void Setup()
        {
            _mockScheduledAssessmentRepository = new Mock<IScheduledAssessmentRepository>();
            _mockScheduledAssessmentService = new Mock<IScheduledAssessmentService>();
            _mockLogger = new Mock<ILogger<ScheduledAssessmentController>>();

            _controller = new ScheduledAssessmentController(
                _mockScheduledAssessmentRepository.Object,
                _mockScheduledAssessmentService.Object,
                _mockLogger.Object
            );
        }



        [Test]
        public async Task GetAttendedStudents_ReturnsOkResult_WithListOfTraineeStatusDTO()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var attendedStudents = new List<TraineeStatusDTO>
    {
        new TraineeStatusDTO { TraineeId = 1, Name = "John Doe", Score = 85 },
        new TraineeStatusDTO { TraineeId = 2, Name = "Jane Smith", Score = 90 }
    };

            // Mock the service method to return the list of TraineeStatusDTO
            _mockScheduledAssessmentService
                .Setup(service => service.GetAttendedStudentsAsync(scheduledAssessmentId))
                .ReturnsAsync(attendedStudents);

            // Act
            var actionResult = await _controller.GetAttendedStudents(scheduledAssessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result); // Ensure the result is of type OkObjectResult

            var okResult = actionResult.Result as OkObjectResult; 
            Assert.IsNotNull(okResult); 
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var response = okResult.Value as IEnumerable<TraineeStatusDTO>; // Extract the data
            Assert.IsNotNull(response); 
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual("John Doe", response.First().Name);
            Assert.AreEqual("Jane Smith", response.Last().Name);
        }




        [Test]
        public async Task GetAttendedStudents_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int scheduledAssessmentId = 1;

            // Mock the service method to throw an exception
            _mockScheduledAssessmentService
                .Setup(service => service.GetAttendedStudentsAsync(scheduledAssessmentId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var actionResult = await _controller.GetAttendedStudents(scheduledAssessmentId);

            // Assert
            var result = actionResult.Result as ObjectResult; // Cast to ObjectResult
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode); 

            var response = result.Value as ApiResponse; // Extract the response data
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("Error retrieving attended students from database.", response.Message.FirstOrDefault());
        }



        [Test]
        public async Task GetAbsentStudents_ReturnsOkResult_WithListOfTraineeStatusDTO()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var absentStudents = new List<TraineeStatusDTO>
    {
        new TraineeStatusDTO { TraineeId = 1, Name = "Alice", Score = 0 },
        new TraineeStatusDTO { TraineeId = 2, Name = "Bob", Score = 0 }
    };

            _mockScheduledAssessmentService
                .Setup(service => service.GetAbsentStudentsAsync(scheduledAssessmentId))
                .ReturnsAsync(absentStudents);

            // Act
            var actionResult = await _controller.GetAbsentStudents(scheduledAssessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result); // Ensure the result is of type OkObjectResult

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode); 

            var response = okResult.Value as IEnumerable<TraineeStatusDTO>;
            Assert.IsNotNull(response); 
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual("Alice", response.First().Name); 
            Assert.AreEqual("Bob", response.Last().Name);
        }

        [Test]
        public async Task GetAbsentStudents_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var scheduledAssessmentId = 1;
            _mockScheduledAssessmentService
                .Setup(service => service.GetAbsentStudentsAsync(scheduledAssessmentId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var actionResult = await _controller.GetAbsentStudents(scheduledAssessmentId);

            // Assert
            // Check if the result is an instance of ActionResult<IEnumerable<TraineeStatusDTO>>
            Assert.IsInstanceOf<ActionResult<IEnumerable<TraineeStatusDTO>>>(actionResult);

            var result = actionResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);

            var apiResponse = result.Value as ApiResponse;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.IsSuccess);
            Assert.AreEqual("Error retrieving absent students from database.", apiResponse.Message.First());
        }





        [Test]
        public async Task GetTraineeAnswerDetails_ReturnsCorrectData()
        {
            // Arrange
            int traineeId = 1;
            int scheduledAssessmentId = 1;

            var traineeAnswerDetails = new List<TraineeAnswerDetailDTO>
            {
                new TraineeAnswerDetailDTO
                {
                    QuestionId = 1,
                    Answer = "Paris",
                    IsCorrect = true,
                    Score = 10,
                    QuestionText = "What is the capital of France?",
                    QuestionType = "MCQ",
                    Points = 10,
                    QuestionOptions = new QuestionOptionDTO
                    {
                        Option1 = "Paris",
                        Option2 = "London",
                        Option3 = "Berlin",
                        Option4 = "Madrid",
                        CorrectAnswer = "Paris"
                    }
                }
            };

            
            _mockScheduledAssessmentService
                .Setup(service => service.GetTraineeAnswerDetailsAsync(traineeId, scheduledAssessmentId))
                .ReturnsAsync(traineeAnswerDetails);

            // Act
            var actionResult = await _controller.GetTraineeAnswerDetails(traineeId, scheduledAssessmentId);

            // Assert
            var objectResult = actionResult as ObjectResult; 
       
            var response = objectResult.Value as ApiResponse;
            Assert.IsNotNull(response); 
            Assert.IsTrue(response.IsSuccess);
            var details = response.Result as IEnumerable<TraineeAnswerDetailDTO>; 
            Assert.IsNotNull(details); 
            Assert.AreEqual(1, details.Count()); 
            Assert.AreEqual("What is the capital of France?", details.First().QuestionText); 
            Assert.AreEqual("MCQ", details.First().QuestionType); 
            Assert.AreEqual("Paris", details.First().QuestionOptions.CorrectAnswer);
        }

        [Test]
        public async Task GetTraineeAnswerDetails_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int traineeId = 1;
            int scheduledAssessmentId = 1;

            
            _mockScheduledAssessmentService
                .Setup(service => service.GetTraineeAnswerDetailsAsync(traineeId, scheduledAssessmentId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var actionResult = await _controller.GetTraineeAnswerDetails(traineeId, scheduledAssessmentId);

            // Assert
            var objectResult = actionResult as ObjectResult; 
            Assert.IsNotNull(objectResult, "Expected ObjectResult but got null"); 

            
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode, "Status code is not InternalServerError");

            var response = objectResult.Value as ApiResponse;
            Assert.IsNotNull(response, "Expected ApiResponse but got null");

           
            Assert.IsFalse(response.IsSuccess, "Expected IsSuccess to be false"); 
            Assert.IsTrue(response.Message.Contains("Error retrieving trainee answer details: Database error"), "Error message does not match");
        }




        [Test]
        public async Task UpdateScheduledAssessmentStatus_ReturnsOk_WhenAssessmentIsUpdated()
        {
            // Arrange
            int assessmentId = 1;
            var statusUpdateDTO = new UpdateScheduledAssessmentStatusDTO
            {
                Status = AssessmentStatus.Evaluated
            };

            var scheduledAssessment = new ScheduledAssessment
            {
                ScheduledAssessmentId = assessmentId,
                Status = AssessmentStatus.Completed 
            };

            // Mock the repository methods
            _mockScheduledAssessmentRepository
                .Setup(repo => repo.GetByIdAsync(assessmentId))
                .ReturnsAsync(scheduledAssessment);

            _mockScheduledAssessmentRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<ScheduledAssessment>()))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _controller.UpdateScheduledAssessmentStatus(assessmentId, statusUpdateDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as ObjectResult;
            Assert.IsNotNull(okResult); 
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var response = okResult.Value as ApiResponse;
            Assert.IsNotNull(response); 
            Assert.IsTrue(response.IsSuccess); 
            var updatedAssessment = response.Result as ScheduledAssessment; 
            Assert.IsNotNull(updatedAssessment); 
            Assert.AreEqual(statusUpdateDTO.Status, updatedAssessment.Status);
        }


        [Test]
        public async Task UpdateScheduledAssessmentStatus_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int assessmentId = 1;
            var statusUpdateDTO = new UpdateScheduledAssessmentStatusDTO
            {
                Status = AssessmentStatus.Completed
            };

            // Mock the repository methods to throw an exception
            _mockScheduledAssessmentRepository
                .Setup(repo => repo.GetByIdAsync(assessmentId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var actionResult = await _controller.UpdateScheduledAssessmentStatus(assessmentId, statusUpdateDTO);

            // Assert
            Assert.IsInstanceOf<ActionResult<ApiResponse>>(actionResult); 
            var objectResult = actionResult.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }


        [Test]
        public async Task GetStudentCountByAssessment_ReturnsStudentCount_WhenValidAssessmentIdProvided()
        {
            // Arrange
            int assessmentId = 1;
            int expectedStudentCount = 10;

            _mockScheduledAssessmentRepository.Setup(repo => repo.GetStudentCountByAssessmentIdAsync(assessmentId))
                .ReturnsAsync(expectedStudentCount);

            // Act
            var result = await _controller.GetStudentCountByAssessment(assessmentId) as ActionResult<int>;

            // Assert
            Assert.IsNotNull(result, "Expected ActionResult<int> but got null");

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode, "Expected Status 200 OK but got different status code");
            Assert.AreEqual(expectedStudentCount, okResult.Value, "Expected student count value does not match");
        }

        [Test]
        public async Task GetStudentCountByAssessment_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            int assessmentId = 1;

            var exception = new Exception("Test exception");

            _mockScheduledAssessmentRepository.Setup(repo => repo.GetStudentCountByAssessmentIdAsync(assessmentId))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetStudentCountByAssessment(assessmentId) as ActionResult<int>;

            // Assert
            Assert.IsNotNull(result, "Expected ActionResult<int> but got null");

            var internalServerErrorResult = result.Result as ObjectResult;
            Assert.IsNotNull(internalServerErrorResult, "Expected ObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode, "Expected Status 500 Internal Server Error but got different status code");
        }

        [Test]
        public async Task GetStudentCountByAssessment_ReturnsNotFound_WhenNoStudentsFound()
        {
            // Arrange
            int assessmentId = 1;
            int expectedStudentCount = 0;

            _mockScheduledAssessmentRepository.Setup(repo => repo.GetStudentCountByAssessmentIdAsync(assessmentId))
                .ReturnsAsync(expectedStudentCount);

            // Act
            var result = await _controller.GetStudentCountByAssessment(assessmentId) as ActionResult<int>;

            // Assert
            Assert.IsNotNull(result, "Expected ActionResult<int> but got null");

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult, "Expected NotFoundObjectResult but got null");
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode, "Expected Status 404 Not Found but got different status code");
        }


        [Test]
        public async Task GetScheduledAssessmentDetails_ValidId_ReturnsOkResult()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var scheduledAssessmentDetails = new ScheduledAssessmentDetailsDTO
            {
                ScheduledAssessmentId = scheduledAssessmentId,
                MaximumScore = 100,
                TotalTrainees = 10,
                TraineesAttended = 8,
                Absentees = 2,
                AssessmentDate = DateTime.Now
            };

            _mockScheduledAssessmentService.Setup(s => s.GetScheduledAssessmentDetailsAsync(scheduledAssessmentId))
                .ReturnsAsync(scheduledAssessmentDetails);

            // Act
            var result = await _controller.GetScheduledAssessmentDetails(scheduledAssessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(scheduledAssessmentDetails, okResult.Value);
        }

        [Test]
        public void GetScheduledAssessmentDetails_InvalidId_ThrowsException()
        {
            // Arrange
            int scheduledAssessmentId = 99;
            _mockScheduledAssessmentService.Setup(s => s.GetScheduledAssessmentDetailsAsync(scheduledAssessmentId))
                .ThrowsAsync(new Exception("Scheduled assessment not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _controller.GetScheduledAssessmentDetails(scheduledAssessmentId));
            Assert.AreEqual("Scheduled assessment not found", ex.Message);
        }

        [Test]
        public async Task GetAssessmentTableByScheduledAssessmentId_ValidId_ReturnsOkResult()
        {
            // Arrange
            int scheduledAssessmentId = 1;
            var assessmentTableDto = new AssessmentTableDTO
            {
                AssessmentId = 1,
                AssessmentName = "Assessment 1",
                BatchName = "Batch A",
                CreatedOn = DateTime.Now,
                ScheduledDate = DateTime.Now,
                Status = "Scheduled"
            };

            _mockScheduledAssessmentRepository.Setup(r => r.GetAssessmentTableByScheduledAssessmentId(scheduledAssessmentId))
                .ReturnsAsync(assessmentTableDto);

            // Act
            var result = await _controller.GetAssessmentTableByScheduledAssessmentId(scheduledAssessmentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(assessmentTableDto, okResult.Value);
        }

        [Test]
        public async Task GetAssessmentTableByScheduledAssessmentId_InvalidId_ReturnsNotFound()
        {
            // Arrange
            int scheduledAssessmentId = 99;
            _mockScheduledAssessmentRepository.Setup(r => r.GetAssessmentTableByScheduledAssessmentId(scheduledAssessmentId))
                .ReturnsAsync((AssessmentTableDTO)null);

            // Act
            var result = await _controller.GetAssessmentTableByScheduledAssessmentId(scheduledAssessmentId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

    }
}
