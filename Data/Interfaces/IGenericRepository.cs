using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Challenges.Data.Interfaces
{
    // IGenericRepository defines common CRUD operations for any entity.
    public interface IGenericRepository<T> where T : class
    {
        // Get an entity by its ID asynchronously.
        Task<T?> GetByIdAsync(int id);

        // Get all entities of type T asynchronously.
        Task<IEnumerable<T>> GetAllAsync();

        // Find entities asynchronously based on a predicate (LINQ expression).
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add a new entity to the context asynchronously.
        Task AddAsync(T entity);

        // Add a range of entities to the context asynchronously.
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update an existing entity in the context.
        void Update(T entity);

        // Remove an entity from the context.
        void Remove(T entity);

        // Remove a range of entities from the context.
        void RemoveRange(IEnumerable<T> entities);

        // Check if an entity with the given predicate exists.
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
