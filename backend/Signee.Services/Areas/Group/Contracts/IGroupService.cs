namespace Signee.Services.Areas.Group.Contracts;
using Group =  Domain.Entities.Group.Group;
public interface IGroupService
{
    Task<IEnumerable<Group>> GetAllGroups();
    
    Task<Group?> GetGroupById(string id);
    
    Task AddToGroup(Group group);
    
    Task CreateGroup(Group group);
    
    Task UpdateGroup(Group id);
    
    Task DeleteById(string id);
    
    Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync();
    
    Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync();
    
    Task<IEnumerable<Group>> GetByDisplayId(string displayId);
    
    Task<IEnumerable<Group>> GetByUserId(string userId);
    
    Task AddDisplayToGroup(string groupId, string displayId);
    
    Task RemoveDisplayFromGroup(string groupId, string displayId);
}