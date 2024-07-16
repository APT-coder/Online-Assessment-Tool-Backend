using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(APIContext context) : base(context) // Ensure APIContext is passed to base constructor
        {

        }
    }
}
