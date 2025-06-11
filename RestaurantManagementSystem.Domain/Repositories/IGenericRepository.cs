using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first entity that matches the specified criteria.
        /// </summary>
        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> criteria,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a queryable collection for dynamic queries.
        /// </summary>
        Task<IQueryable<T>> QueryAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds entities that match the specified criteria.
        /// </summary>
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> criteria,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a range of entities.
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Updates a range of entities.
        /// </summary>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Deletes a range of entities.
        /// </summary>
        void DeleteRange(IEnumerable<T> entities);
    }
}