using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Signee.Domain.Entities.Common;
using Signee.Domain.Exceptions;
using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Infrastructure.PostgreSql.Common;

public class PostgreSqlRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public PostgreSqlRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
    
    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate) => (await FindAllAsync(predicate)).FirstOrDefault();

    public async Task<T> GetByIdAsync(object id)
    {
        var entity =  await _dbSet.FindAsync(id);

        if (entity == null)
            throw new EntityNotExistException($"{typeof(T).Name} with ID: {id} does not exist!");

        return entity;
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesTransactionalAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await SaveChangesTransactionalAsync();
    }
    
    public async Task DeleteByIdAsync(object id)
    {
        var entity = await GetByIdAsync(id);
        await DeleteAsync(entity);
    }
    
    public async Task UpdateAsync(T entity)
    {
        // Checks entity existence
        await GetByIdAsync(entity.Id);
        
        _dbSet.Update(entity);
        await SaveChangesTransactionalAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        // Checks entity existence
        await GetByIdAsync(entity.Id);
        
        _dbSet.Remove(entity);
        await SaveChangesTransactionalAsync();
    }
    
    public async Task SaveChangesTransactionalAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}