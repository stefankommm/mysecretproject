using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.Display;
using Display = Domain.Entities.Display.Display;

public interface IDisplayRepository : IGenericRepository<Display>
{
    // TODO Add some display specific methods
    Task<IEnumerable<Display>> GetByGroupId(string groupId);
    
    Task<IEnumerable<Display>> GetByName(string name);
    
    Task<IEnumerable<Display>> GetOwnedByUserId(string userId);
    
    
}