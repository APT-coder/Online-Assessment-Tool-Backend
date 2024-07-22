using Microsoft.EntityFrameworkCore.Storage;
using OnlineAssessmentTool.Models;
using System.Collections.Generic;

namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IUserRepository : IRepository<Users>
    {

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Users> GetByIdAsync(int id);
    }
}
