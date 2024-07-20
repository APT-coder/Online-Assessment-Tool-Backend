using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using AutoMapper;
using System.Net;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Repository;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;
        private readonly IQuestionService _questionService;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public AssessmentController(IAssessmentService assessmentService, IQuestionService questionService, IAssessmentRepository assessmentRepository, IQuestionRepository questionRepository)
        {
            _assessmentService = assessmentService;
            _questionService = questionService;
            _assessmentRepository = assessmentRepository ?? throw new ArgumentNullException(nameof(assessmentRepository));
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssessments()
        {
            var response = new ApiResponse();
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
            var assessment = await _assessmentService.GetAssessmentByIdAsync(assessmentId);
            if (assessment == null)
            {
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


        [HttpPost]
        public async Task<IActionResult> CreateAssessment([FromBody] AssessmentDTO assessmentDTO)
        {
            var response = new ApiResponse();
            var assessment = await _assessmentService.CreateAssessmentAsync(assessmentDTO);
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
            if (question == null)
            {
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
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            existingAssessment.AssessmentName = assessmentDTO.AssessmentName;
            existingAssessment.CreatedBy = assessmentDTO.CreatedBy;
            _assessmentRepository.UpdateAsync(existingAssessment);
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
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            response.IsSuccess = true;
            response.Result = question;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Question updated successfully.");
            return Ok(response);
        }

        [HttpDelete("{assessmentId}")]
        public async Task<IActionResult> DeleteAssessment(int assessmentId)
        {
            var response = new ApiResponse();
            var assessment = await _assessmentService.GetAssessmentByIdAsync(assessmentId);
            if (assessment == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message.Add("Assessment not found.");
                return NotFound(response);
            }
            await _assessmentService.DeleteAssessmentAsync(assessmentId);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessment deleted successfully.");
            return Ok(response);
        }

        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var response = new ApiResponse();
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            await _questionService.DeleteQuestionAsync(questionId);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Question deleted successfully.");
            return Ok(response);
        }
    }
}
