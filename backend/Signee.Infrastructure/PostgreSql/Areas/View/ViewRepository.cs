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

    public async Task<View?> GetViewByTimeAsync(DateTime time)
        => await FindAsync(view => view.From <= time && view.To >= time);

    public async Task<IEnumerable<View>> GetAllByGroupIdAsync(string groupId)
        => await FindAllAsync(view => view.GroupId == groupId);
}