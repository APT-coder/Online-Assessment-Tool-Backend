using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
