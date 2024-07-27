using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;
using OnlineAssessmentTool.Services.IService;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AssessmentScoreController : Controller
    {
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;
        private readonly IAssessmentScoreService _assessmentScoreService;
        public AssessmentScoreController(IAssessmentScoreRepository assessmentScoreRepository, IAssessmentScoreService assessmentScoreService)
        {
            _assessmentScoreRepository = assessmentScoreRepository;
            _assessmentScoreService = assessmentScoreService;
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

        [HttpGet("score-distribution/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetScoreDistribution(int assessmentId)
        {
            var scoreDistribution = await _assessmentScoreRepository.GetScoreDistributionAsync(assessmentId);
            return Ok(scoreDistribution);
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

        [HttpPut("update-assessment-scores")]
        public async Task<ActionResult<ApiResponse>> UpdateAssessmentScores([FromBody] UpdateAssessmentScoreDTO updateAssessmentScoreDTO)
        {
            try
            {
                await _assessmentScoreService.UpdateAssessmentScoresAsync(updateAssessmentScoreDTO.AssessmentScores);

                return Ok(new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = new List<string> { "Assessment scores updated successfully." } });
            }
            catch (Exception ex)
            {
                // Log exception (ex) if necessary
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating assessment scores." } });
            }
        }
    }
}
