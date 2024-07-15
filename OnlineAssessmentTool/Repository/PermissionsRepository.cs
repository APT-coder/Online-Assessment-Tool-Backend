using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly APIContext _context;

        public PermissionsRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission> GetAsync(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task CreateAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await SaveAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Entry(permission).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task RemoveAsync(Permission permission)
        {
            _context.Permissions.Remove(permission);
            await SaveAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Permissions.AnyAsync(p => p.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
