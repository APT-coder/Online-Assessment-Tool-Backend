

using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using AutoMapper;
using System.Net;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IMapper _mapper;

        public AssessmentController(IAssessmentRepository assessmentRepository, IMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssessments()
        {
            var response = new ApiResponse();
            var assessments = await _assessmentRepository.GetAllAssessmentsAsync();
            var assessmentsDTO = _mapper.Map<IEnumerable<AssessmentDTO>>(assessments);

            response.IsSuccess = true;
            response.Result = assessmentsDTO;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessments retrieved successfully.");

            return Ok(response);
        }

        [HttpGet("{assessmentId}")]
        public async Task<IActionResult> GetAssessment(int assessmentId)
        {
            var response = new ApiResponse();

            var assessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
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

        [HttpGet("questions/{questionId}")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {

            var response = new ApiResponse();
            var question = await _assessmentRepository.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }
            response.IsSuccess = true;
            response.Result = _mapper.Map<QuestionDTO>(question);
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessment([FromBody] AssessmentDTO assessmentDTO)
        {
            var response = new ApiResponse();

            var assessment = _mapper.Map<Assessment>(assessmentDTO);
            assessment.CreatedOn = DateTime.UtcNow;
            await _assessmentRepository.AddAssessmentAsync(assessment);

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

            var question = _mapper.Map<Question>(questionDTO);
            question.CreatedOn = DateTime.UtcNow;
            question.AssessmentId = assessmentId;

            var assessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
            if (assessment == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message.Add("Assessment not found.");
                return NotFound(response);
            }

            assessment.Questions.Add(question);
            await _assessmentRepository.AddAssessmentAsync(assessment);
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
            await _assessmentRepository.UpdateAssessmentAsync(existingAssessment);
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
            var existingQuestion = await _assessmentRepository.GetQuestionByIdAsync(questionId);
            if (existingQuestion == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }


            existingQuestion.QuestionText = questionDTO.QuestionText;
            existingQuestion.QuestionType = questionDTO.QuestionType;
            existingQuestion.Points = questionDTO.Points;
            existingQuestion.QuestionOptions = _mapper.Map<List<QuestionOption>>(questionDTO.QuestionOptions);

            await _assessmentRepository.UpdateQuestionAsync(existingQuestion);
            response.IsSuccess = true;
            response.Result = existingQuestion;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Question updated successfully.");

            return Ok(response);
        }

        [HttpDelete("{assessmentId}")]
        public async Task<IActionResult> DeleteAssessment(int assessmentId)
        {
            var response = new ApiResponse();
            var assessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
            if (assessment == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message.Add("Assessment not found.");
                return NotFound(response);
            }

            await _assessmentRepository.DeleteAssessmentAsync(assessmentId);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Assessment deleted successfully.");

            return Ok(response);
        }

        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var response = new ApiResponse();
            var question = await _assessmentRepository.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            await _assessmentRepository.DeleteQuestionAsync(questionId);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message.Add("Question deleted successfully.");

            return Ok(response);
        }
    }
}
