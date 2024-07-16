namespace OnlineAssessmentTool.Repository.IRepository
{
    public interface IRepository<T>
    {
        Task<bool> AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> SaveAsync();
    }

}