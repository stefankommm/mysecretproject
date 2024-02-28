namespace Signee.Services.Areas.Group.Contracts;
using Group =  Domain.Entities.Group.Group;
public interface IGroupService
{
    Task<IEnumerable<Group>> GetAllAsync();
    
    Task<Group?> GetByIdAsync(string id);
    
    Task AddAsync(Group group);
    
    Task UpdateByIdAsync(string id);
    
    Task DeleteByIdAsync(string id);
    
    Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync();
    
    Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync();
    
    Task<IEnumerable<Group>> GetByDisplayIdAsync(string displayId);
    
    Task<IEnumerable<Group>> GetByUserIdAsync(string userId);
}