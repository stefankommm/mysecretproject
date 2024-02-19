using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Signee.Infrastructure.PostgreSql.Contracts;

namespace Signee.Infrastructure.PostgreSql.Data;

public class PostgreSqlRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public PostgreSqlRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

    public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteByIdAsync(object id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            await DeleteAsync(entity);
    }
    
    public async Task UpdateByIdAsync(object id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            await DeleteAsync(entity);
    }
    
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}