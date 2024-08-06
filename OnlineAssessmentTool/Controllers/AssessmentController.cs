using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using AutoMapper;
using System.Net;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;
        private readonly IQuestionService _questionService;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssessmentController> _logger;

        public AssessmentController(IAssessmentService assessmentService, IQuestionService questionService, IAssessmentRepository assessmentRepository, IQuestionRepository questionRepository, ILogger<AssessmentController> logger)
        {
            _assessmentService = assessmentService;
            _questionService = questionService;
            _assessmentRepository = assessmentRepository;
            _questionRepository = questionRepository;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAssessments()
        {
            var response = new ApiResponse();
            _logger.LogInformation("Retrieving all assessments.");
            var assessments = await _assessmentRepository.GetAllAsync();
            response.IsSuccess = true;
            response.Result = assessments;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessments retrieved successfully.");
            return Ok(response);
        }

        [HttpGet("{assessmentId}")]
        public async Task<IActionResult> GetAssessment(int assessmentId)
        {
            var response = new ApiResponse();
            _logger.LogInformation("Retrieving assessment with Id {assessmentId}.",assessmentId);
            var assessment = await _assessmentService.GetAssessmentByIdAsync(assessmentId);
            if (assessment == null)
            {
                _logger.LogWarning("Assessment with ID {Id} not found.", assessmentId);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            response.IsSuccess = true;
            response.Result = assessment;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

        [HttpGet("questions/{questionId}/options")]
        public async Task<IActionResult> GetQuestionOptions(int questionId)
        {
            var question = await _questionRepository.GetQuestionByIdAsync(questionId);

            if (question == null)
            {
                _logger.LogWarning("Question Options with ID {Id} not found.", questionId );
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
                Result = question.QuestionOptions
            };

            return Ok(response);
        }

        [HttpGet("overview")]
        public async Task<IResult> GetAllAssessmentOverviews()
        {
            var response = new ApiResponse();
            try
            {
                _logger.LogInformation("Fetching all assessment overviews");
                var overviews = await _assessmentRepository.GetAllAssessmentOverviewsAsync();
                if (overviews == null || !overviews.Any())
                {
                    _logger.LogWarning(" Assessments are not found");
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message.Add("No assessments found.");
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = overviews;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message.Add("Assessment overviews retrieved successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving assessment overviews");
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message.Add("Error retrieving assessment overviews: " + ex.Message);
            }

            return Results.Ok(response);
        }


        [HttpGet("highperformers/{assessmentId}")]
        public async Task<IActionResult> GetHighPerformers(int assessmentId)
        {
            _logger.LogInformation("Fetching high performers in an assessment with id {assessmentId}",assessmentId);
            var result = await _assessmentRepository.GetHighPerformersByAssessmentIdAsync(assessmentId);
            Console.WriteLine(result);
            return Ok(result);
        }

        [HttpGet("lowperformers/{assessmentId}")]
        public async Task<IActionResult> GetLowPerformers(int assessmentId)
        {
            _logger.LogInformation("Fetching low performers in an assessment with id {assessmentId}", assessmentId);
            var result = await _assessmentRepository.GetLowPerformersByAssessmentIdAsync(assessmentId);
            return Ok(result);
        }

        [HttpGet("{trainerId}")]
        public async Task<ActionResult<List<AssessmentTableDTO>>> GetAssessmentTable(int trainerId)
        {
            var assessments = await _assessmentRepository.GetAssessmentsForTrainer(trainerId);
            if (assessments == null || assessments.Count == 0)
            {
                return NotFound();
            }
            return Ok(assessments);
        }


        [HttpGet("GetTraineeAssessmentDetails/{scheduledAssessmentId}")]
        public async Task<ActionResult<List<TraineeAssessmentTableDTO>>> GetTraineeAssessmentDetails(int scheduledAssessmentId)
        {
            var result = await _assessmentRepository.GetTraineeAssessmentDetails(scheduledAssessmentId);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessment([FromBody] AssessmentDTO assessmentDTO)
        {
            var response = new ApiResponse();
            var assessment = await _assessmentService.CreateAssessmentAsync(assessmentDTO);
            _logger.LogInformation("Assessment created successfully");
            response.IsSuccess = true;
            response.Result = assessment;
            response.StatusCode = HttpStatusCode.Created;
            response.Message.Add("Assessment created successfully.");
            return CreatedAtAction(nameof(GetAssessment), new { assessmentId = assessment.AssessmentId }, response);
        }

        [HttpPost("{assessmentId}/questions")]
        public async Task<IActionResult> AddQuestionToAssessment(int assessmentId, [FromBody] QuestionDTO questionDTO)
        {
            var response = new ApiResponse();
            var question = await _questionService.AddQuestionToAssessmentAsync(assessmentId, questionDTO);
            _logger.LogInformation("Questions added to Assessment with id {assessmentId} successfully",assessmentId);
            if (question == null)
            {
                _logger.LogWarning("Assessment with id {assessmentId} not found", assessmentId);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message.Add("Assessment not found.");
                return NotFound(response);
            }
            response.IsSuccess = true;
            response.Result = question;
            response.StatusCode = HttpStatusCode.Created;
            response.Message.Add("Question added successfully.");
            return CreatedAtAction(nameof(GetAssessment), new { assessmentId = question.AssessmentId }, response);
        }

        [HttpPut("{assessmentId}")]
        public async Task<IActionResult> UpdateAssessment(int assessmentId, [FromBody] AssessmentDTO assessmentDTO)
        {
            var response = new ApiResponse();
            var existingAssessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
            if (existingAssessment == null)
            {
                _logger.LogWarning("Assessment with id {assessmentId} not found",assessmentId);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            existingAssessment.AssessmentName = assessmentDTO.AssessmentName;
            existingAssessment.CreatedBy = assessmentDTO.CreatedBy;
            existingAssessment.TotalScore = assessmentDTO.TotalScore;
            _assessmentRepository?.UpdateAsync(existingAssessment);
            _logger.LogInformation("Assessment with id {assessmentId} updated successfully",assessmentId);
            response.IsSuccess = true;
            response.Result = existingAssessment;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessment updated successfully.");

            return Ok(response);
        }

        [HttpPut("{assessmentId}")]
        public async Task<IActionResult> UpdateAssessmentTotalScore(int assessmentId, [FromBody] UpdateAssessmentTotalScoreDTO assessmentDTO)
        {
            var response = new ApiResponse();
            var existingAssessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
            if (existingAssessment == null)
            {
                _logger.LogWarning("Assessment with id {assessmentId} not found", assessmentId);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message.Add("Assessment not found."); // Ensure this line is present
                return NotFound(response);
            }
            existingAssessment.TotalScore = assessmentDTO.TotalScore;
            _assessmentRepository?.UpdateAsync(existingAssessment);
            _logger.LogInformation("Assessment with id {assessmentId} updated successfully", assessmentId);
            response.IsSuccess = true;
            response.Result = existingAssessment;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessment updated successfully.");

            return Ok(response);
        }


        [HttpPut("questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] QuestionDTO questionDTO)
        {
            var response = new ApiResponse();
            var question = await _questionService.UpdateQuestionAsync(questionId, questionDTO);
            if (question == null)
            {
                _logger.LogInformation("Question with id {quuestionId} not found", questionId);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            response.IsSuccess = true;
            response.Result = question;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Question updated successfully.");
            _logger.LogInformation("Question with id {quuestionId} updated successfully", questionId);
            return Ok(response);

        }

        [HttpDelete("{assessmentId}")]
        public async Task<IActionResult> DeleteAssessment(int assessmentId)
        {
            var response = new ApiResponse();
            var assessment = await _assessmentService.GetAssessmentByIdAsync(assessmentId);
            if (assessment == null)
            {
                _logger.LogWarning($"Assessment with id {assessmentId} was not found");
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message.Add("Assessment not found.");
                return NotFound(response);
            }
            await _assessmentService.DeleteAssessmentAsync(assessmentId);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessment deleted successfully.");
            _logger.LogInformation("Assessment with id {assessmentId} deleted successfully",assessmentId);
            return Ok(response);
        }

        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var response = new ApiResponse();
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                _logger.LogWarning($"Question with id {questionId} was not found");
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            await _questionService.DeleteQuestionAsync(questionId);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Question deleted successfully.");
            _logger.LogInformation("question with id {questionId} deleted successfully", questionId);
            return Ok(response);
        }
    }
}
