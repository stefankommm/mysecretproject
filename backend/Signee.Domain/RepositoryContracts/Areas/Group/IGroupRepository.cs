using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.Group;
using Group = Domain.Entities.Group.Group;
public interface IGroupRepository : IGenericRepository<Group>
{
    Task<IEnumerable<Group>> GetAll();
    
    Task<Group?> GetById(string groupId);
    
    Task<IEnumerable<Group>> GetAllWithSingleDisplay();
    
    Task<IEnumerable<Group>> GetAllWithMultipleDisplays();
    
    
}