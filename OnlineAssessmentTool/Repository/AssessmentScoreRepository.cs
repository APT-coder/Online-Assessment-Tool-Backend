using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentScoreRepository : Repository<AssessmentScore>, IAssessmentScoreRepository
    {
        public AssessmentScoreRepository(APIContext context) : base(context) { }
    }
}
