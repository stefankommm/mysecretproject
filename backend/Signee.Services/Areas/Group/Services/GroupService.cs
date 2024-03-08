using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Areas.User.Contracts;
using Signee.Services.Areas.View.Contracts;
using Signee.Services.Auth.Contracts;

namespace Signee.Services.Areas.Group.Services;

using Group = Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;

public class GroupService : IGroupService
{
    private readonly IDisplayService _displayService;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserService _userService;
    private readonly IViewService _viewService;
    private readonly IUserContextProvider _userContextProvider;

    public GroupService(IUserService userService, IGroupRepository groupRepository, IDisplayService displayService,
        IViewService viewService, IUserContextProvider userContextProvider)
    {
        _userService = userService;
        _groupRepository = groupRepository;
        _displayService = displayService;
        _viewService = viewService;
        _userContextProvider = userContextProvider;
    }
    
    public async Task AddAsync(Group group)
    {
        group.UserId = _userContextProvider.GetCurrentUserId();
        if(group.Name == null)
            throw new InvalidOperationException("Group name cannot be null");

        var user = await _userService.GetByIdAsync(group.UserId);
        await _userService.UpdateAsync(user);
        await _groupRepository.AddAsync(group);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Users groups (All if ADMIN)</returns>
    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        if(_userContextProvider.IsAdmin())
            return await _groupRepository.GetAllAsync();
        return await _groupRepository.FindAllAsync(g => g.UserId == _userContextProvider.GetCurrentUserId());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Group with Given ID if the owner is calling (ADMIN -> returns if exist)</returns>
    /// <exception cref="InvalidOperationException">ID not Found</exception>
    /// <exception cref="InvalidOperationException">User is not owner of the display</exception>
    public async Task<Group> GetByIdAsync(string id)
    {
        var group = await _groupRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Group with id: {id} not found");
        if (!_userContextProvider.IsAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        return group;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>All Users groups with one display(ADMIN- All =1d)</returns>
    public async Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync()
    {
        var all = await GetAllAsync();
        return all.Where(g => g.Displays.Count == 1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>All Users groups >1 display (ADMIN - All >1d)</returns>
    public async Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync()
    {
        var all = await GetAllAsync();
        return all.Where(g => g.Displays.Count > 1);
    }
    

    /// <summary>
    /// USER: Updates the group if the user is the owner
    /// ADMIN: Updates the group if it exists
    /// </summary>
    /// <param name="g"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task UpdateAsync(Group g)
    {
        if(g.Id == null)            
            throw new InvalidOperationException("Group ID cannot be null -> Can't Update");
        
        var group = await GetByIdAsync(g.Id ?? string.Empty)
            ?? throw new InvalidOperationException($"Group with id: {g.Id} not found");
        
        if(!_userContextProvider.IsAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        await _groupRepository.UpdateAsync(group);
    }

    /// <summary>
    /// USER: Deletes the group if the user is the owner
    /// ADMIN: Deletes the group if it exists
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DeleteByIdAsync(string id)
    {
        var group = await GetByIdAsync(id);
        if (!_userContextProvider.IsAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        if (group.Displays.Count > 0)
            throw new InvalidOperationException("Group contains displays, cannot delete");
        if (group.Views.Count > 0)
            throw new InvalidOperationException("Group contains views, cannot delete");
        
        await _groupRepository.DeleteByIdAsync(id);
    }
    
    /// <summary>
    /// USER: Gets all groups with the given user ID if the user is the owner
    /// ADMIN: Gets all groups with the given user ID
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<IEnumerable<Group>> GetByUserIdAsync(string userId)
    {
        if (!_userContextProvider.IsAdmin() && userId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You can't get other user's groups.");
        return await _groupRepository.FindAllAsync(g => g.UserId == userId);
    }

    /// <summary>
    /// USER: Gets the group with the given display ID if the user is the owner and d exists
    /// ADMIN: Gets the group with the given display ID if exists
    /// </summary>
    /// <param name="displayId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Group> GetByDisplayIdAsync(string displayId)
    {
        var display = await _displayService.GetByIdAsync(displayId)
            ?? throw new InvalidOperationException($"Display with id: {displayId} not found");
        if (!_userContextProvider.IsAdmin() && display.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this display");

        return await _groupRepository.FindAsync(g => g.Displays.Contains(display));
    }

    /// <summary>
    /// USER: Gets the group with the given view ID if the user is the owner and v exists
    /// ADMIN: Gets the group with the given view ID if exists
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="displayId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task AddDisplayToGroupAsync(string groupId, string displayId)
    {
        var display = await _displayService.GetByIdAsync(displayId);
        var group = await GetByIdAsync(groupId);

        if (group == null)
            throw new InvalidOperationException("Group or display not found");
        if(display == null)
            throw new InvalidOperationException("Display not found");

        if (!_userContextProvider.IsAdmin())
        {
            if (display.UserId != _userContextProvider.GetCurrentUserId())
                throw new InvalidOperationException("You are not the owner of this display");
            if (group.UserId != _userContextProvider.GetCurrentUserId())
                throw new InvalidOperationException("You are not the owner of this group");
        }
        
        if(group.Displays.Contains(display))
            throw new InvalidOperationException("Display is already part of the group");
        
        group.Displays.Add(display);
        await _groupRepository.UpdateAsync(group);
    }

    
    /// <summary>
    /// USER: Removes the display from the group if the user is the owner and d exists
    /// ADMIN: Removes the display from the group if exists
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="displayId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task RemoveDisplayFromGroupAsync(string groupId, string displayId)
    {
        var display = await _displayService.GetByIdAsync(displayId)
            ?? throw new InvalidOperationException($"Display with id: {displayId} not found");
        var group = await GetByIdAsync(groupId)
            ?? throw new InvalidOperationException($"Group with id: {groupId} not found");
        
        if (!_userContextProvider.IsAdmin())
        {
            if (display.UserId != _userContextProvider.GetCurrentUserId())
                throw new InvalidOperationException("You are not the owner of this display");
            if (group.UserId != _userContextProvider.GetCurrentUserId())
                throw new InvalidOperationException("You are not the owner of this group");
        }
        
        if(!group.Displays.Contains(display))
            throw new InvalidOperationException("Display is not part of the group");
        
        group.Displays.Remove(display);
        await _groupRepository.UpdateAsync(group);
    }

    public async Task AddViewToGroupAsync(string groupId, string viewId)
    {
        // Deprecated
        // #TODO Remove. The view now must have it's corresponding group
        // #TODO: This should be only for admin? The user should only add and delete views
        var group = await GetByIdAsync(groupId)
            ?? throw new InvalidOperationException($"Group with id: {groupId} not found");
        var view = await _viewService.GetByIdAsync(viewId)
            ?? throw new InvalidOperationException($"View with id: {viewId} not found");
        
        if(!_userContextProvider.IsAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
                throw new InvalidOperationException("You are not the owner of this group");
        
        if (view.GroupId != null)
            throw new InvalidOperationException($"View with id: {viewId} already belongs to a group");
        
        group.Views.Add(view);
    }

    public async Task RemoveViewFromGroupAsync(string groupId, string viewId)
    {
        // Deprecated
        // #TODO Remove. The view now must have it's corresponding group
        var group = await GetByIdAsync(groupId)
                    ?? throw new InvalidOperationException($"Group with id: {groupId} not found");
        var view = await _viewService.GetByIdAsync(viewId)
                   ?? throw new InvalidOperationException($"View with id: {viewId} not found");
        
        if(!_userContextProvider.IsAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        if (view.GroupId != groupId)
            throw new InvalidOperationException($"View with id: {viewId} already belongs to a group");
        
        group.Views.Remove(view);
    }

    public async Task<View?> GetCurrentViewAsync(string groupId)
    {
        var group = await GetByIdAsync(groupId);
        if(group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        if (group?.Views.Count == 0)
            return null;

        var actualViews = group?.Views
            .Select(v => v?.To == null ? v : v.From < DateTime.Now && v.To > DateTime.Now ? v : null).ToList();
        return actualViews?.FirstOrDefault();
    }
}