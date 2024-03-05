using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.Display;
using Display = Domain.Entities.Display.Display;

public interface IDisplayRepository : IGenericRepository<Display>
{
    Task<IEnumerable<Display>> GetAllByGroupIdAsync(string groupId);
    
    Task<IEnumerable<Display>> GetAllByNameAsync(string name);
    
    /// <param name="userId"></param>
    /// <returns>Displays that user with certain ID owns</returns>
    Task<IEnumerable<Display>> GetOwnedByUserIdAsync(string userId);
}