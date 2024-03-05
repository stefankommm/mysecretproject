using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.Group;
using Group = Domain.Entities.Group.Group;
public interface IGroupRepository : IGenericRepository<Group>
{
    Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync();
    
    Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync();
}