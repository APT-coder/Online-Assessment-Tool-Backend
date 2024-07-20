using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(APIContext context) : base(context)
        {

        }




        public bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}

