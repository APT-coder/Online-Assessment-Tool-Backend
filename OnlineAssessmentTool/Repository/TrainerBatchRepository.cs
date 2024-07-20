using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class TrainerBatchRepository : Repository<TrainerBatch>, ITrainerBatchRepository
    {
        public TrainerBatchRepository(APIContext context) : base(context)
        {

        }

    }
}
