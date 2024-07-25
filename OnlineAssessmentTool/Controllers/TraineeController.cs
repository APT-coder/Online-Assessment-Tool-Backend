using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Repository.IRepository;

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
    }
}
