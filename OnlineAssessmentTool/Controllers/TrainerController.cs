using AutoMapper;
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
    public class TrainerController :ControllerBase
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IMapper _mapper;


        public TrainerController(ITrainerRepository trainerRepository, IMapper mapper)
        {
            _trainerRepository = trainerRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainerList()
        {
            try
            {
                var trainerList = await _trainerRepository.GetAllTrainersAsync();
                Console.WriteLine(trainerList);

                if (trainerList == null || !trainerList.Any())
                {
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
