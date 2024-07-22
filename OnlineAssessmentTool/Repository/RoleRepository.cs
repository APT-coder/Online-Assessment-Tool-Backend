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
        /*public async Task<Role> GetRoleWithPermissionsAsync(int roleId)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .SingleOrDefaultAsync(r => r.Id == roleId);
        }*/

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

    }
}

