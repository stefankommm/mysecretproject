using Microsoft.EntityFrameworkCore;
using Signee.Domain.Exceptions;
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

    public async Task<Display> GetDisplayByPairingCodeAsync(Guid pairingCode)
    {
        var display = await FindAsync(display => display.PairingCode == pairingCode);
        
        if (display == null)
            throw new EntityNotExistException($"Display with pairing code: {pairingCode} does not exist!");

        return display;
    }

    public async Task<IEnumerable<Display>> GetAllByGroupIdAsync(string groupId) 
        => await FindAllAsync(display => display.GroupId == groupId);

    public async Task<IEnumerable<Display>> GetAllByNameAsync(string name)
        => await FindAllAsync(display => display.Name == name);
    
    public async Task<IEnumerable<Display>> GetOwnedByUserIdAsync(string userId)
    {
        // var userGroups = await _dbContext.Groups
        //     .Where(group => group.CreatedById.Id == userId)
        //     .ToListAsync();
        // var displays = await _dbContext.Displays.Where(d => userGroups.Find(g => g.Id == d.GroupId) != null).ToListAsync();
        // return displays;
        throw new NotImplementedException();
    }
}