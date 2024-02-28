using Microsoft.EntityFrameworkCore;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Infrastructure.PostgreSql.Common;

namespace Signee.Infrastructure.PostgreSql.Areas.Group;
using Group = Domain.Entities.Group.Group;
public class GroupRepository : PostgreSqlRepository<Group>, IGroupRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public GroupRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Group>> GetAll()
    {
        return await _dbContext.Groups.ToListAsync();
    }

    public async Task<Group?> GetById(string groupId)
    {
        return await _dbContext.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
    }

    public async Task<IEnumerable<Group>> GetAllWithSingleDisplay()
    {
        return await _dbContext.Groups.Where(group => group.Displays!.Count == 1).ToListAsync();
    }

    public async Task<IEnumerable<Group>> GetAllWithMultipleDisplays()
    {
        return await _dbContext.Groups.Where(group => group.Displays!.Count > 1).ToListAsync();
    }
}