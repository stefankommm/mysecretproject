using Microsoft.EntityFrameworkCore;
using Signee.Domain.RepositoryContracts.Areas.View;
using Signee.Infrastructure.PostgreSql.Common;

namespace Signee.Infrastructure.PostgreSql.Areas.View;
using View = Domain.Entities.View.View;

public class ViewRepository : PostgreSqlRepository<View>, IViewRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ViewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<View>> GetAll()
    {
        return await _dbContext.Views.ToListAsync();
    }

    public async Task<View?> GetById(string viewId)
    {
        return await _dbContext.Views.FirstOrDefaultAsync(view => view.Id == viewId);
    }

    public async Task<View?> GetViewAtTime(DateTime time)
    {
        return await _dbContext.Views.FirstOrDefaultAsync(view => view.From <= time && view.To >= time);
    }

    public async Task<IEnumerable<View>> GetAllByGroupId(string groupId)
    {
        return await _dbContext.Views.Where(view => view.GroupId == groupId).ToListAsync();
    }
}