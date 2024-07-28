using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;
using Microsoft.Extensions.Logging; // Add this using directive

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainerController> _logger; // Add this

        public TrainerController(ITrainerRepository trainerRepository, IMapper mapper, ILogger<TrainerController> logger) // Add logger to constructor
        {
            _trainerRepository = trainerRepository;
            _mapper = mapper;
            _logger = logger; // Initialize logger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainerList()
        {
            try
            {
                _logger.LogInformation("Fetching all trainers.");
                var trainerList = await _trainerRepository.GetAllTrainersAsync();
                _logger.LogInformation("Fetched {trainerCount} trainers.", trainerList.Count());

                if (trainerList == null || !trainerList.Any())
                {
                    _logger.LogWarning("No trainers found.");
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No trainers found." }
                    });
                }

                return Ok(trainerList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trainers.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving trainers from database." }
                });
            }
        }
    }
}
