using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services;
using OnlineAssessmentTool.Services.IService;
using System.Globalization;
using System.Net;

namespace OnlineAssessmentTool.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScheduledAssessmentController : ControllerBase
    {
        private readonly IScheduledAssessmentRepository _scheduledAssessmentRepository;
        private readonly IScheduledAssessmentService _scheduledAssessmentService;
        private readonly ILogger<RolesController> _logger;
        public ScheduledAssessmentController(IScheduledAssessmentRepository scheduledAssessmentRepository, IScheduledAssessmentService scheduledAssessmentService, ILogger<RolesController> logger)
        {
            _scheduledAssessmentRepository = scheduledAssessmentRepository;
            _scheduledAssessmentService = scheduledAssessmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledAssessment>>> GetScheduledAssessment()
        {
            try
            {
                _logger.LogInformation("Getting Scheduled Assessments");
                var scheduledAssessment = await _scheduledAssessmentRepository.GetAllAsync();
                Console.WriteLine(scheduledAssessment);

                if (scheduledAssessment == null || !scheduledAssessment.Any())
                {
                    _logger.LogWarning("No assessment found.");
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No assessment found." }
                    });
                }

                return Ok(scheduledAssessment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the assessment.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving assessment from database." }
                });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetScheduledByUserId(int userId)
        {
            _logger.LogInformation("Fetching assessment for user with ID {userid}", userId);
            var scheduled = await _scheduledAssessmentRepository.GetScheduledAssessmentsByUserIdAsync(userId);

            if (scheduled == null)
            {
                _logger.LogWarning("Questions for user with ID {userId} not found", userId);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = { "Question not found." }
                });
            }

            var response = new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = scheduled
            };

            return Ok(response);
        }

        [HttpGet("studentcount")]
        public async Task<ActionResult<int>> GetStudentCountByAssessment([FromQuery] int assessmentId)
        {
            try
            {
                _logger.LogInformation("Fetching the number of students who attended assessment by assessmentid {assessmentId}",assessmentId);
                int studentCount = await _scheduledAssessmentRepository.GetStudentCountByAssessmentIdAsync(assessmentId);

                if (studentCount == 0)
                {
                    _logger.LogWarning("No students for assessment by id {assessmentId}",assessmentId);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No students found for the specified assessment." }
                    });
                }

                return Ok(studentCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching count of students.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "An error occurred while retrieving the student count." }
                });
            }
        }

        [HttpGet("attended-students/{scheduledAssessmentId}")]
        public async Task<ActionResult<IEnumerable<TraineeStatusDTO>>> GetAttendedStudents(int scheduledAssessmentId)
        {
            try
            {
                _logger.LogInformation("Fetching attended students for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                var attendedStudents = await _scheduledAssessmentService.GetAttendedStudentsAsync(scheduledAssessmentId);

                if (attendedStudents == null || !attendedStudents.Any())
                {
                    _logger.LogWarning("No attended students found for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No students found." }
                    });
                }
                _logger.LogInformation("Successfully fetched attended students for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                return Ok(attendedStudents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving attended students for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving attended students from database." }
                });
            }
        }

        [HttpGet("absent-students/{scheduledAssessmentId}")]
        public async Task<ActionResult<IEnumerable<TraineeStatusDTO>>> GetAbsentStudents(int scheduledAssessmentId)
        {
            try
            {
                _logger.LogInformation("Fetching absent students for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                var absentStudents = await _scheduledAssessmentService.GetAbsentStudentsAsync(scheduledAssessmentId);

                if (absentStudents == null || !absentStudents.Any())
                {
                    _logger.LogWarning("No absent students found for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No students found." }
                    });
                }
                _logger.LogInformation("Successfully fetched absent students for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                return Ok(absentStudents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving absent students for scheduledAssessmentId {ScheduledAssessmentId}.", scheduledAssessmentId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving absent students from database." }
                });
            }
        }


        [HttpGet("trainee/{traineeId}/scheduledAssessment/{scheduledAssessmentId}")]
        public async Task<IActionResult> GetTraineeAnswerDetails(int traineeId, int scheduledAssessmentId)
        {
            var response = new ApiResponse();
            try
            {
                _logger.LogInformation("Fetching answer details for traineeId {TraineeId} and scheduledAssessmentId {ScheduledAssessmentId}.", traineeId, scheduledAssessmentId);
                var details = await _scheduledAssessmentService.GetTraineeAnswerDetailsAsync(traineeId, scheduledAssessmentId);
                if (details == null || !details.Any())
                {
                    _logger.LogWarning("No answers found for traineeId {TraineeId} and scheduledAssessmentId {ScheduledAssessmentId}.", traineeId, scheduledAssessmentId);
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message.Add("No answers found for the specified trainee and assessment.");
                    return NotFound(response);
                }

                response.IsSuccess = true;
                response.Result = details;
                response.StatusCode = HttpStatusCode.OK;
                response.Message.Add("Trainee answer details retrieved successfully.");
                _logger.LogInformation("Successfully fetched answer details for traineeId {TraineeId} and scheduledAssessmentId {ScheduledAssessmentId}.", traineeId, scheduledAssessmentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving trainee answer details for traineeId {TraineeId} and scheduledAssessmentId {ScheduledAssessmentId}.", traineeId, scheduledAssessmentId);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message.Add("Error retrieving trainee answer details: " + ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet("AssessmentTable/{scheduledAssessmentId}")]
        public async Task<IActionResult> GetAssessmentTableByScheduledAssessmentId(int scheduledAssessmentId)
        {
            var dtos = await _scheduledAssessmentRepository.GetAssessmentTableByScheduledAssessmentId(scheduledAssessmentId);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostScheduledAssessment([FromBody] ScheduledAssessmentDTO scheduledAssessmentDTO)
        {
            try
            {
                if (!TimeSpan.TryParseExact(scheduledAssessmentDTO.AssessmentDuration, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out var assessmentDuration))
                {
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Invalid format for AssessmentDuration. Expected format is 'hh:mm:ss'." }
                    });
                }
                _logger.LogInformation("Adding new scheduled assessment.");
                var scheduledAssessment = new ScheduledAssessment
                {
                    BatchId = scheduledAssessmentDTO.BatchId,
                    AssessmentId = scheduledAssessmentDTO.AssessmentId,
                    ScheduledDate = scheduledAssessmentDTO.ScheduledDate,
                    AssessmentDuration = assessmentDuration,
                    StartDate = scheduledAssessmentDTO.StartDate,
                    EndDate = scheduledAssessmentDTO.EndDate,
                    StartTime = scheduledAssessmentDTO.StartTime,
                    EndTime = scheduledAssessmentDTO.EndTime,
                    Status = scheduledAssessmentDTO.Status,
                    CanRandomizeQuestion = scheduledAssessmentDTO.CanRandomizeQuestion,
                    CanDisplayResult = scheduledAssessmentDTO.CanDisplayResult,
                    CanSubmitBeforeEnd = scheduledAssessmentDTO.CanSubmitBeforeEnd,
                    Link = scheduledAssessmentDTO.Link,
                };

                await _scheduledAssessmentRepository.AddAsync(scheduledAssessment);
                return CreatedAtAction(nameof(GetScheduledAssessment), new { id = scheduledAssessment.ScheduledAssessmentId }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = scheduledAssessment });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new scheduled assessment.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error creating scheduled assessment." } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteScheduledAssessment(int id)
        {
            try
            {
                var scheduledAssessment = await _scheduledAssessmentRepository.GetByIdAsync(id);
                if (scheduledAssessment == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Assessment not found." } });
                }

                await _scheduledAssessmentRepository.DeleteAsync(scheduledAssessment);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error deleting assessment." } });
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutScheduledAssessment(int id, ScheduledAssessmentDTO scheduledAssessmentDTO)
        {
            try
            {
                _logger.LogInformation("Deleting scheduled assessment with ID {Id}.", id);
                var scheduledAssessment = await _scheduledAssessmentRepository.GetByIdAsync(id);
                if (scheduledAssessment == null)
                {
                    _logger.LogWarning("Failed to delete scheduled assessment with ID {Id}.", id);
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Assessment not found." } });
                }

                if (!TimeSpan.TryParseExact(scheduledAssessmentDTO.AssessmentDuration, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out var assessmentDuration))
                {
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Invalid format for AssessmentDuration. Expected format is 'hh:mm:ss'." }
                    });
                }

                scheduledAssessment.BatchId = scheduledAssessmentDTO.BatchId;
                scheduledAssessment.AssessmentId = scheduledAssessmentDTO.AssessmentId;
                scheduledAssessment.ScheduledDate = scheduledAssessmentDTO.ScheduledDate;
                scheduledAssessment.AssessmentDuration = assessmentDuration;
                scheduledAssessment.StartDate = scheduledAssessmentDTO.StartDate;
                scheduledAssessment.EndDate = scheduledAssessmentDTO.EndDate;
                scheduledAssessment.StartTime = scheduledAssessmentDTO.StartTime;
                scheduledAssessment.EndTime = scheduledAssessmentDTO.EndTime;
                scheduledAssessment.Status = scheduledAssessmentDTO.Status;
                scheduledAssessment.CanRandomizeQuestion = scheduledAssessmentDTO.CanRandomizeQuestion;
                scheduledAssessment.CanDisplayResult = scheduledAssessmentDTO.CanDisplayResult;
                scheduledAssessment.CanSubmitBeforeEnd = scheduledAssessmentDTO.CanSubmitBeforeEnd;
                scheduledAssessment.Link = scheduledAssessmentDTO.Link;
                await _scheduledAssessmentRepository.UpdateAsync(scheduledAssessment);
                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting scheduled assessment with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating assessment." } });
            }
        }

        [HttpPut("update-status/{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateScheduledAssessmentStatus(int id, [FromBody] UpdateScheduledAssessmentStatusDTO statusUpdateDTO)
        {
            try
            {
                var scheduledAssessment = await _scheduledAssessmentRepository.GetByIdAsync(id);
                if (scheduledAssessment == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Scheduled Assessment not found." }
                    });
                }

                // Update the status of the scheduled assessment
                scheduledAssessment.Status = statusUpdateDTO.Status;

                // Save changes to the database
                await _scheduledAssessmentRepository.UpdateAsync(scheduledAssessment);

                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = scheduledAssessment
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error updating status of scheduled assessment." }
                });
            }
        }
    }
}
