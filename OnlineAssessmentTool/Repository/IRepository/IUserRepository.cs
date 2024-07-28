using Microsoft.EntityFrameworkCore.Storage;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Users> GetByIdAsync(int id);
    }
}
