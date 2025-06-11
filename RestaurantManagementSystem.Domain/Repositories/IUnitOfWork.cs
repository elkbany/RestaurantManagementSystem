using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        IGenericRepository<T> Repository<T>() where T : class;
        /// <summary>
        /// Saves all changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}