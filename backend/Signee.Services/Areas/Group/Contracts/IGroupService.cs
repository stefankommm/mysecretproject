namespace Signee.Services.Areas.Group.Contracts;
using Group =  Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;


public interface IGroupService
{
    
    Task AddAsync(Group g);
    
    Task<IEnumerable<Group>> GetAllAsync();
    
    Task<Group> GetByIdAsync(string id);
    
    Task<Group> UpdateAsync(Group g);
    
    Task DeleteByIdAsync(string id);
    
    /// ----- DISPLAY METHODS -----

    Task AddDisplayToGroupAsync(string groupId, string displayId);
    
    Task RemoveDisplayFromGroupAsync(string groupId, string displayId);
    
    /// ----- VIEW METHODS -----
    
    Task<View?> GetCurrentViewAsync(string groupId);
}