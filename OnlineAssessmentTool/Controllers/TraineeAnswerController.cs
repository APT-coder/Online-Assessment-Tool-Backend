﻿using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;
using OnlineAssessmentTool.Services.IService;
using static OnlineAssessmentTool.Models.DTO.CheckTraineeAnswerExitsDTO;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TraineeAnswerController : ControllerBase
    {
        private readonly ITraineeAnswerRepository _traineeAnswerRepository;
        private readonly IAssessmentPostService _assessmentPostService;

        public TraineeAnswerController(ITraineeAnswerRepository traineeAnswerRepository, IAssessmentPostService assessmentPostService)
        {
            _traineeAnswerRepository = traineeAnswerRepository;
            _assessmentPostService = assessmentPostService;
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

        [HttpPost("{userId}")]
        public async Task<IActionResult> AssessmentSubmit([FromBody] List<PostAssessmentDTO> questions, int userId)
        {

            var traineeAnswer = await _assessmentPostService.ProcessTraineeAnswers(questions, userId);
            return Ok();
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
                return CreatedAtAction(nameof(GetTraineeAnswer), new { id = traineeAnswer.TraineeAnswerId }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = traineeAnswer });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error creating answer." } });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating answer." } });
            }
        }

        [HttpPut("updateScore")]
        public async Task<IActionResult> UpdateScore([FromBody] UpdateScoreDTO updateScoreDTO)
        {
            try
            {
                var traineeAnswer = await _traineeAnswerRepository.GetTraineeAnswerAsync(updateScoreDTO.ScheduledAssessmentId, updateScoreDTO.TraineeId, updateScoreDTO.QuestionId);

                if (traineeAnswer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "TraineeAnswer not found." }
                    });
                }

                traineeAnswer.Score = updateScoreDTO.Score;
                await _traineeAnswerRepository.UpdateTraineeAnswerAsync(traineeAnswer);
                return Ok(traineeAnswer);
            }
            catch (Exception ex)
            {
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
