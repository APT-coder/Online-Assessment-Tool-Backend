using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;
using OnlineAssessmentTool.Services.IService;
using static OnlineAssessmentTool.Models.DTO.CheckTraineeAnswerExitsDTO;
using Microsoft.Extensions.Logging; // Add this using directive


namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TraineeAnswerController : ControllerBase
    {
        private readonly ITraineeAnswerRepository _traineeAnswerRepository;
        private readonly IAssessmentPostService _assessmentPostService;
        private readonly ILogger<TraineeAnswerController> _logger; // Add this

        public TraineeAnswerController(
            ITraineeAnswerRepository traineeAnswerRepository,
            IAssessmentPostService assessmentPostService,
            ILogger<TraineeAnswerController> logger) // Add logger to constructor
        {
            _traineeAnswerRepository = traineeAnswerRepository;
            _assessmentPostService = assessmentPostService;
            _logger = logger; // Initialize logger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraineeAnswer>>> GetTraineeAnswer()
        {
            try
            {
                _logger.LogInformation("Fetching all trainee answers.");
                var traineeAnswer = await _traineeAnswerRepository.GetAllAsync();
                _logger.LogInformation("Fetched {answerCount} trainee answers.", traineeAnswer.Count());

                if (traineeAnswer == null || !traineeAnswer.Any())
                {
                    _logger.LogWarning("No trainee answers found.");
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No answer found." }
                    });
                }

                return Ok(traineeAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trainee answers.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving answer from database." }
                });
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AssessmentSubmit([FromBody] List<PostAssessmentDTO> questions, int userId)
        {
            try
            {
                _logger.LogInformation("Submitting assessment for user {userId}.", userId);
                await _assessmentPostService.ProcessTraineeAnswers(questions, userId);
                _logger.LogInformation("Assessment submitted successfully for user {userId}.", userId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting the assessment for user {userId}.", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error processing the assessment." }
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CheckTraineeAnswerExists([FromBody] CheckTraineeAnswerDTO checkTraineeAnswerDTO)
        {
            var response = new ApiResponse();
            try
            {
                bool exists = await _traineeAnswerRepository
                    .CheckTraineeAnswerExistsAsync(checkTraineeAnswerDTO.ScheduledAssessmentId, checkTraineeAnswerDTO.UserId);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message.Add("Check completed successfully.");
                response.Result = new { Exists = exists };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message.Add("Error checking trainee answer.");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostTraineeAnswer([FromBody] TraineeAnswerDTO traineeAnswerDTO)
        {
            try
            {
                var traineeAnswer = new TraineeAnswer
                {
                    ScheduledAssessmentId = traineeAnswerDTO.ScheduledAssessmentId,
                    TraineeId = traineeAnswerDTO.TraineeId,
                    QuestionId = traineeAnswerDTO.QuestionId,
                    Answer = traineeAnswerDTO.Answer,
                    IsCorrect = traineeAnswerDTO.IsCorrect,
                    Score = traineeAnswerDTO.Score
                };

                await _traineeAnswerRepository.AddAsync(traineeAnswer);
                _logger.LogInformation("Trainee answer created with ID {id}.", traineeAnswer.TraineeAnswerId);
                return CreatedAtAction(nameof(GetTraineeAnswer), new { id = traineeAnswer.TraineeAnswerId }, new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = traineeAnswer
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a trainee answer.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error creating answer." }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteTraineeAnswer(int id)
        {
            try
            {
                var traineeAnswer = await _traineeAnswerRepository.GetByIdAsync(id);
                if (traineeAnswer == null)
                {
                    _logger.LogWarning("Trainee answer with ID {id} not found.", id);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Answer not found." }
                    });
                }

                await _traineeAnswerRepository.DeleteAsync(traineeAnswer);
                _logger.LogInformation("Trainee answer with ID {id} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting trainee answer with ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error deleting answer." }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutTraineeAnswer(int id, TraineeAnswerDTO traineeAnswerDTO)
        {
            try
            {
                var traineeAnswer = await _traineeAnswerRepository.GetByIdAsync(id);
                if (traineeAnswer == null)
                {
                    _logger.LogWarning("Trainee answer with ID {id} not found for update.", id);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Answer not found." }
                    });
                }

                traineeAnswer.ScheduledAssessmentId = traineeAnswerDTO.ScheduledAssessmentId;
                traineeAnswer.TraineeId = traineeAnswerDTO.TraineeId;
                traineeAnswer.QuestionId = traineeAnswerDTO.QuestionId;
                traineeAnswer.Answer = traineeAnswerDTO.Answer;
                traineeAnswer.IsCorrect = traineeAnswerDTO.IsCorrect;
                traineeAnswer.Score = traineeAnswerDTO.Score;
                await _traineeAnswerRepository.UpdateAsync(traineeAnswer);
                _logger.LogInformation("Trainee answer with ID {id} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating trainee answer with ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error updating answer." }
                });
            }
        }

        [HttpPut("updateScore")]
        public async Task<IActionResult> UpdateScore([FromBody] UpdateScoreDTO updateScoreDTO)
        {
            try
            {
                _logger.LogInformation("Updating score for ScheduledAssessmentId {scheduledAssessmentId}, TraineeId {traineeId}, QuestionId {questionId}.",
                    updateScoreDTO.ScheduledAssessmentId, updateScoreDTO.TraineeId, updateScoreDTO.QuestionId);

                var traineeAnswer = await _traineeAnswerRepository.GetTraineeAnswerAsync(
                    updateScoreDTO.ScheduledAssessmentId,
                    updateScoreDTO.TraineeId,
                    updateScoreDTO.QuestionId
                );

                if (traineeAnswer == null)
                {
                    _logger.LogWarning("TraineeAnswer not found for ScheduledAssessmentId {scheduledAssessmentId}, TraineeId {traineeId}, QuestionId {questionId}.",
                        updateScoreDTO.ScheduledAssessmentId, updateScoreDTO.TraineeId, updateScoreDTO.QuestionId);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "TraineeAnswer not found." }
                    });
                }

                traineeAnswer.Score = updateScoreDTO.Score;
                await _traineeAnswerRepository.UpdateTraineeAnswerAsync(traineeAnswer);
                _logger.LogInformation("Score updated successfully for ScheduledAssessmentId {scheduledAssessmentId}, TraineeId {traineeId}, QuestionId {questionId}.",
                    updateScoreDTO.ScheduledAssessmentId, updateScoreDTO.TraineeId, updateScoreDTO.QuestionId);
                return Ok(traineeAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the score for ScheduledAssessmentId {scheduledAssessmentId}, TraineeId {traineeId}, QuestionId {questionId}.",
                    updateScoreDTO.ScheduledAssessmentId, updateScoreDTO.TraineeId, updateScoreDTO.QuestionId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "An error occurred while trying to update the score." }
                });
            }
        }
    }
}
