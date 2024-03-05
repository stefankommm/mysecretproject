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

    public async Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync() 
        => await FindAllAsync(group => group.Displays!.Count == 1);

    public async Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync()
        => await FindAllAsync(group => group.Displays!.Count > 1);
}