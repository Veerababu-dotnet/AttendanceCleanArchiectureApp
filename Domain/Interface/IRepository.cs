
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task DeleteAsync(T entity);

        IQueryable<T> Query(); // for custom queries

        // Save changes to database
        Task SaveAsync();

        // Find entities by condition
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
