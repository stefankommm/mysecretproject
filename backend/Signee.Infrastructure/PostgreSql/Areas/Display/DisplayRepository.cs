using Microsoft.EntityFrameworkCore;
using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Infrastructure.PostgreSql.Common;

namespace Signee.Infrastructure.PostgreSql.Areas.Display;
using Display = Domain.Entities.Display.Display;

public class DisplayRepository : PostgreSqlRepository<Display>, IDisplayRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DisplayRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    // TODO Add some specific methods for accessing display data in the future
    public async Task<IEnumerable<Display>> GetByGroupId(string groupId)
    {
        return await _dbContext.Displays.Where(display => display.GroupId == groupId).ToListAsync();
    }

    public async Task<IEnumerable<Display>> GetByName(string name)
    {
        return await _dbContext.Displays.Where(display => display.Name == name).ToListAsync();
    }

    public async Task<IEnumerable<Display>> GetOwnedByUserId(string userId)
    {
        // var userGroups = await _dbContext.Groups
        //     .Where(group => group.CreatedById.Id == userId)
        //     .ToListAsync();
        // var displays = await _dbContext.Displays.Where(d => userGroups.Find(g => g.Id == d.GroupId) != null).ToListAsync();
        // return displays;
        throw new NotImplementedException();
    }
}