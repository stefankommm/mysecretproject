namespace Signee.Services.Areas.Group.Contracts;
using Group =  Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;

public interface IGroupService
{
    
    Task<IEnumerable<Group>> GetAllAsync();
    
    Task<Group> GetByIdAsync(string id);
    
    Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync();

    Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync();
    
    Task AddAsync(Group g);
    
    Task UpdateAsync(Group g);
    
    Task DeleteByIdAsync(string id);
    
    /// ----- USER METHODS -----
    
    /// <param name="userId"></param>
    /// <returns>All groups that belong to user with give ID</returns>
    Task<IEnumerable<Group>> GetByUserIdAsync(string userId);
    
    /// <param name="displayId"></param>
    /// <returns>All groups that contain display with given ID</returns>
    Task<IEnumerable<Group>> GetByDisplayIdAsync(string displayId);
    
    /// ----- DISPLAY METHODS -----

    Task AddDisplayToGroupAsync(string groupId, string displayId);
    
    Task RemoveDisplayFromGroupAsync(string groupId, string displayId);
    
    /// ----- VIEW METHODS -----
     
    Task AddViewToGroupAsync(string groupId, string viewId);
    
    Task RemoveViewFromGroupAsync(string groupId, string viewId);
    
    Task<View?> GetCurrentViewAsync(string groupId);
}