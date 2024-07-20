using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TraineeAnswerController : ControllerBase
    {
        private readonly ITraineeAnswerRepository _traineeAnswerRepository;

        public TraineeAnswerController(ITraineeAnswerRepository traineeAnswerRepository)
        {
            _traineeAnswerRepository = traineeAnswerRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraineeAnswer>>> GetTraineeAnswer()
        {
            try
            {
                var traineeAnswer = await _traineeAnswerRepository.GetAllAsync();
                Console.WriteLine(traineeAnswer);

                if (traineeAnswer == null || !traineeAnswer.Any())
                {
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving answer from database." }
                });
            }
        }



        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostTraineeAnswer([FromBody] TraineeAnswerDTO traineeAnswerDTO)
        {
            try
            {
                // Explicitly cast the Status from DTO to the model's Status enum
                ;

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

                return CreatedAtAction(nameof(GetTraineeAnswer), new { id = traineeAnswer.TraineeAnswerId }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = traineeAnswer });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error creating scheduled assessment." } });
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
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Answer not found." } });
                }

                await _traineeAnswerRepository.DeleteAsync(traineeAnswer);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error deleting answer." } });
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
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Answer not found." } });
                }

                traineeAnswer.ScheduledAssessmentId = traineeAnswerDTO.ScheduledAssessmentId;
                traineeAnswer.TraineeId = traineeAnswerDTO.TraineeId;
                traineeAnswer.QuestionId = traineeAnswerDTO.QuestionId;
                traineeAnswer.Answer = traineeAnswerDTO.Answer;
                traineeAnswer.IsCorrect = traineeAnswerDTO.IsCorrect;
                traineeAnswer.Score = traineeAnswerDTO.Score;



                await _traineeAnswerRepository.UpdateAsync(traineeAnswer);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating answer." } });
            }
        }

    }
}
