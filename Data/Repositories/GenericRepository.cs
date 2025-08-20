using Challenges.Data;
using Challenges.Data.Interfaces;
using Challenges.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Challenges.Repositories
{
    // GenericRepository provides concrete implementations for the IGenericRepository interface.
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ChallengesDbContext _context; // Protected to be accessible by derived repositories
        protected readonly DbSet<T> _dbSet; // DbSet for the current entity type

        public GenericRepository(ChallengesDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // Initialize DbSet for the specific entity type
        }

        // Add a single entity asynchronously.
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Add a range of entities asynchronously.
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Find entities based on a predicate, without tracking changes.
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        // Get all entities of type T, without tracking changes.
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        // Get an entity by its ID asynchronously.
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Remove a single entity.
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Remove a range of entities.
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // Update an entity (its state will be set to Modified).
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Check if an entity with the given predicate exists asynchronously.
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
