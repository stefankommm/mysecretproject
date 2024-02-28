using System.Linq.Expressions;

namespace Signee.Domain.RepositoryContracts.Common;

/// <summary>
/// Generic repository contract for common data access operations.
/// </summary>
/// <typeparam name="T">The type of entity handled by the repository.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves entities of type <typeparamref name="T"/> that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains filtered entities.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves the entity of type <typeparamref name="T"/> with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(object id);

    /// <summary>
    /// Adds a new entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Adds a collection of entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entities">The collection of entities to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Updates the entity of type <typeparamref name="T"/> with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateByIdAsync(object id);

    /// <summary>
    /// Deletes the entity of type <typeparamref name="T"/> with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteByIdAsync(object id);

    /// <summary>
    /// Updates the specified entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes the specified entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Saves changes made to the database within a transactional context.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveChangesTransactionalAsync();
}