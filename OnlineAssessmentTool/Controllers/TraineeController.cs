using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IMapper _mapper;

        public TraineeController(ITraineeRepository traineeRepository, IMapper mapper)
        {
            _traineeRepository = traineeRepository;
            _mapper = mapper;
        }


        /*   [HttpGet]
           public async Task<IActionResult> GetAllTrainees()
           {
               var response = new ApiResponse();
               var trainees = await _traineeRepository.GetAllTraineesAsync();
               var traineeDTO = _mapper.Map<IEnumerable<TraineeDTO>>(trainee);

               response.IsSuccess = true;
               response.Result = traineeDTO;
               response.StatusCode = HttpStatusCode.OK;
               response.Message.Add("TYrainees retrieved successfully.");

               return Ok(response);
           }*/

    }
}
