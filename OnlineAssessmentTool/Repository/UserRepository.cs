using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class UserRepository: Repository<Users>, IUserRepository
    {
        public UserRepository(APIContext context) : base(context)
        {

        }

      
               public async Task<IEnumerable<Users>> GetUsersByRoleAsync(string roleName)
                {
            return null;
            
                }
        public async Task<int> CreateUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }
    }

}

