using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Domain.RepositoryContracts.Areas.View;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Auth.Contracts;

namespace Signee.Services.Areas.Group.Services;

using Group = Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;
using Display = Domain.Entities.Display.Display;

public class GroupService : IGroupService
{
    private readonly IDisplayRepository _displayRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IViewRepository _viewRepository;
    private readonly IUserContextProvider _userContextProvider;

    public GroupService(IDisplayRepository displayRepository, IGroupRepository groupRepository, IViewRepository viewRepository, 
                        IUserContextProvider userContextProvider)
    {
        _displayRepository = displayRepository;
        _groupRepository = groupRepository;
        _viewRepository = viewRepository;
        _userContextProvider = userContextProvider;
    }
    
    /// <summary>
    /// Checks if user has sufficient privileges (owns group or is admin)
    /// to be able to access/manipulate it
    /// </summary>
    /// <param name="group"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void CheckGroupOwnership(Group group)
    {
        if (!_userContextProvider.isAdmin()  && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group!"); // TODO create ownership exception and return localized resource
    }
    
    // TODO NTH - This method repeats itself, try to refactor in the furute
    /// <summary>
    /// Checks if user has sufficient privileges (owns display or is admin)
    /// to be able to access/manipulate it
    /// </summary>
    /// <param name="display"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void CheckDisplayOwnership(Display display)
    {
        if (!_userContextProvider.isAdmin()  && display.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this display!"); // TODO create ownership exception and return localized resource
    }

    public async Task AddAsync(Group group)
    {
        group.UserId = _userContextProvider.GetCurrentUserId();
        if(group.Name == null)
            throw new InvalidOperationException("Group name cannot be null"); // TODO add localized resource

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
        var group = await _groupRepository.GetByIdAsync(id);
        CheckGroupOwnership(group);
        
        return group;
    }
    
    // TODO change incoming object to some Request object
    public async Task<Group> UpdateAsync(Group updatedGroup)
    {
        var oldGroup = await GetByIdAsync(updatedGroup.Id);
        CheckGroupOwnership(oldGroup);

        if (!_userContextProvider.isAdmin())
        {
            var currentUserId = _userContextProvider.GetCurrentUserId();
            
            if (updatedGroup.UserId != currentUserId)
                throw new InvalidOperationException("U don´t have permission to transfer group ownership");
            
            if (updatedGroup.Displays.Select(d => d.UserId != currentUserId).ToList().Count > 0)
                throw new InvalidOperationException("U don´t have permission to assign unowned displays to group");
        }
        
        await _groupRepository.UpdateAsync(updatedGroup);
        
        return await _groupRepository.GetByIdAsync(updatedGroup.Id);
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
        CheckGroupOwnership(group);
        
        // TODO delete widgets 
        
        foreach (var view in group.Views.ToList())
            await _viewRepository.DeleteByIdAsync(view.Id);
        
        foreach (var display in group.Displays.ToList())
            await _displayRepository.DeleteByIdAsync(display.Id);
        
        await _groupRepository.DeleteByIdAsync(id);
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
        var display = await _displayRepository.GetByIdAsync(displayId);
        CheckDisplayOwnership(display);
        
        var group = await GetByIdAsync(groupId);
        CheckGroupOwnership(group);
        
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
        var display = await _displayRepository.GetByIdAsync(displayId);
        CheckDisplayOwnership(display);
        
        var group = await GetByIdAsync(groupId);
        CheckGroupOwnership(group);
        
        // TODO NTH refactor somehow to return 304 error
        if(!group.Displays.Contains(display))
            throw new InvalidOperationException("Display is not part of the group");
        
        group.Displays.Remove(display);
        await _groupRepository.UpdateAsync(group);
    }

    public async Task<View?> GetCurrentViewAsync(string groupId)
    {
        var group = await GetByIdAsync(groupId);
        CheckGroupOwnership(group);
        
        if (group?.Views.Count == 0)
            return null;

        var actualViews = group?.Views
            .Select(v => v?.To == null ? v : v.From < DateTime.Now && v.To > DateTime.Now ? v : null).ToList();
        return actualViews?.FirstOrDefault();
    }
}