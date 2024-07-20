using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class TraineeAnswerRepository : Repository<TraineeAnswer>, ITraineeAnswerRepository
    {
        public TraineeAnswerRepository(APIContext context) : base(context)
        {

        }


    }
}
