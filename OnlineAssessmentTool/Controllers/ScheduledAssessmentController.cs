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

    [Route("[controller]/[action]")]
    [ApiController]
    public class ScheduledAssessmentController : ControllerBase
    {
        private readonly IScheduledAssessmentRepository _scheduledAssessmentRepository;
        private readonly IScheduledAssessmentService _scheduledAssessmentService;
        public ScheduledAssessmentController(IScheduledAssessmentRepository scheduledAssessmentRepository, IScheduledAssessmentService scheduledAssessmentService)
        {
            _scheduledAssessmentRepository = scheduledAssessmentRepository;
            _scheduledAssessmentService = scheduledAssessmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledAssessment>>> GetScheduledAssessment()
        {
            try
            {
                var scheduledAssessment = await _scheduledAssessmentRepository.GetAllAsync();
                Console.WriteLine(scheduledAssessment);

                if (scheduledAssessment == null || !scheduledAssessment.Any())
                {
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
            var scheduled = await _scheduledAssessmentRepository.GetScheduledAssessmentsByUserIdAsync(userId);

            if (scheduled == null)
            {
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
                int studentCount = await _scheduledAssessmentRepository.GetStudentCountByAssessmentIdAsync(assessmentId);

                if (studentCount == 0)
                {
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
                var attendedStudents = await _scheduledAssessmentService.GetAttendedStudentsAsync(scheduledAssessmentId);

                if (attendedStudents == null || !attendedStudents.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No students found." }
                    });
                }

                return Ok(attendedStudents);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
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
                var absentStudents = await _scheduledAssessmentService.GetAbsentStudentsAsync(scheduledAssessmentId);

                if (absentStudents == null || !absentStudents.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No students found." }
                    });
                }

                return Ok(absentStudents);
            }
            catch (Exception ex)
            {
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
                var details = await _scheduledAssessmentService.GetTraineeAnswerDetailsAsync(traineeId, scheduledAssessmentId);
                if (details == null || !details.Any())
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message.Add("No answers found for the specified trainee and assessment.");
                    return NotFound(response);
                }

                response.IsSuccess = true;
                response.Result = details;
                response.StatusCode = HttpStatusCode.OK;
                response.Message.Add("Trainee answer details retrieved successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message.Add("Error retrieving trainee answer details: " + ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
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
                    CanSubmitBeforeEnd = scheduledAssessmentDTO.CanSubmitBeforeEnd
                };

                await _scheduledAssessmentRepository.AddAsync(scheduledAssessment);
                return CreatedAtAction(nameof(GetScheduledAssessment), new { id = scheduledAssessment.ScheduledAssessmentId }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = scheduledAssessment });
            }
            catch (Exception ex)
            {
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
                var scheduledAssessment = await _scheduledAssessmentRepository.GetByIdAsync(id);
                if (scheduledAssessment == null)
                {
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
                await _scheduledAssessmentRepository.UpdateAsync(scheduledAssessment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating assessment." } });
            }
        }
    }
}
