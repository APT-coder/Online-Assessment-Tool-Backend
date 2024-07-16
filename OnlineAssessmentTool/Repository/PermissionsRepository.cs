using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class PermissionsRepository : Repository<Permission>, IPermissionsRepository
    {
        public PermissionsRepository(APIContext context) : base(context) // Ensure APIContext is passed to base constructor
        {

        }




        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Permissions.AnyAsync(p => p.Id == id);
        }

      
    }

}
