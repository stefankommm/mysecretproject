using System.Linq.Expressions;

namespace Signee.Infrastructure.PostgreSql.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(object id);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateByIdAsync(object id);
    Task DeleteByIdAsync(object id);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}