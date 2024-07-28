using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AssessmentScoreController : Controller
    {
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;

        public AssessmentScoreController(IAssessmentScoreRepository assessmentScoreRepository)
        {
            _assessmentScoreRepository = assessmentScoreRepository;
        }

        [HttpGet("{traineeId}")]
        public async Task<IActionResult> GetAssessmentScoresByTraineeId(int traineeId)
        {
            var response = new ApiResponse();

            try
            {
                var assessmentScores = await _assessmentScoreRepository.GetAssessmentScoresByTraineeIdAsync(traineeId);

                if (assessmentScores == null || assessmentScores.Count == 0)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = new List<string> { "No assessment scores found for the trainee." };
                    return NotFound(response);
                }

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = assessmentScores;
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = new List<string> { "Error retrieving assessment scores." };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentScore>>> GetAssessmentScores()
        {
            try
            {
                var assessmentScores = await _assessmentScoreRepository.GetAllAsync();

                if (assessmentScores == null || !assessmentScores.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No scores found." }
                    });
                }

                return Ok(assessmentScores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving scores from database." }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostAssessmentScore([FromBody] AssessmentScoreDTO assessmentScoreDTO)
        {
            try
            {
                var assessmentScore = new AssessmentScore
                {
                    ScheduledAssessmentId = assessmentScoreDTO.ScheduledAssessmentId,
                    TraineeId = assessmentScoreDTO.TraineeId,
                    AvergeScore = assessmentScoreDTO.AvergeScore,
                    CalculatedOn = assessmentScoreDTO.CalculatedOn
                };

                await _assessmentScoreRepository.AddAsync(assessmentScore);

                return CreatedAtAction(nameof(GetAssessmentScores), new { id = assessmentScore.AssessmentScoreId }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = assessmentScore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error creating assessment score." } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAssessmentScore(int id)
        {
            try
            {
                var assessmentScore = await _assessmentScoreRepository.GetByIdAsync(id);
                if (assessmentScore == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Score not found." } });
                }

                await _assessmentScoreRepository.DeleteAsync(assessmentScore);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error deleting score." } });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutAssessmentScore(int id, AssessmentScoreDTO assessmentScoreDTO)
        {
            try
            {
                var assessmentScore = await _assessmentScoreRepository.GetByIdAsync(id);
                if (assessmentScore == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Score not found." } });
                }

                assessmentScore.ScheduledAssessmentId = assessmentScoreDTO.ScheduledAssessmentId;
                assessmentScore.TraineeId = assessmentScoreDTO.TraineeId;
                assessmentScore.AvergeScore = assessmentScoreDTO.AvergeScore;
                assessmentScore.CalculatedOn = assessmentScoreDTO.CalculatedOn;

                await _assessmentScoreRepository.UpdateAsync(assessmentScore);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating score." } });
            }
        }

        [HttpGet("{assessmentId}")]
        public async Task<IActionResult> GetAssessment(int assessmentId)
        {
            var response = new ApiResponse();

            var assessment = await _assessmentScoreRepository.GetAssessmentByIdAsync(assessmentId);
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
    }
}
