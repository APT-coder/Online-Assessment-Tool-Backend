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
        public async Task<List<Permission>> GetPermissionsByNamesAsync(List<string> permissionNames)
        {
            return await _context.Permissions
                .Where(p => permissionNames.Contains(p.PermissionName))
                .ToListAsync();
        }
        public async Task<List<Permission>> GetByIdsAsync(List<int> permissionIds)
        {
            return await _context.Permissions.Where(p => permissionIds.Contains(p.Id)).ToListAsync();
        }




    }

}
