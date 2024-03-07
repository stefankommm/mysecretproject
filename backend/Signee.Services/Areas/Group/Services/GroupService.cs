using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Areas.User.Contracts;
using Signee.Services.Areas.View.Contracts;

namespace Signee.Services.Areas.Group.Services;

using Group = Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;

public class GroupService : IGroupService
{
    private readonly IDisplayService _displayService;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserService _userService;
    private readonly IViewService _viewService;

    public GroupService(IUserService userService, IGroupRepository groupRepository, IDisplayService displayService,
        IViewService viewService)
    {
        _userService = userService;
        _groupRepository = groupRepository;
        _displayService = displayService;
        _viewService = viewService;
    }
    
    public async Task<Group> AddAsync(Group g, string userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        var group = new Group
        {
            Name = g.Name,
            UserId = user.Id
        };
        await _groupRepository.AddAsync(group);
        return group;
    }
    
    public async Task AddAsync(Group group)
    {
        
        var user = await _userService.GetByIdAsync(group?.UserId ?? string.Empty);
        user.Groups.Add(group);

        await _userService.UpdateAsync(user);
        await _groupRepository.AddAsync(group);
    }
    
    public async Task<IEnumerable<Group>> GetAllAsync() => await _groupRepository.GetAllAsync();

    public async Task<Group> GetByIdAsync(string id)
    {
        var group = await _groupRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Group with id: {id} not found");

        return group;
    }

    public async Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync()
    {
        return await _groupRepository.FindAllAsync(g => g.Displays.Count == 1);
    }

    public async Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync()
    {
        return await _groupRepository.FindAllAsync(g => g.Displays.Count > 1);
    }

    public async Task UpdateAsync(Group g)
    {
        if(g.Id == null)
            throw new InvalidOperationException("Group ID cannot be null -> Can't Update");
        var group = await GetByIdAsync(g.Id ?? string.Empty)
            ?? throw new InvalidOperationException($"Group with id: {g.Id} not found");
        
        // #TODO - Ak volajucim je USER, potom groupy musia byt jedinecne
        
        // #TODO - Ak volajucim je ADMIN, potom je jedno ako sa upravena groupa bude volat  
        
        await _groupRepository.UpdateAsync(group);
    }

    public async Task DeleteByIdAsync(string id)
    {
        await GetByIdAsync(id); // Checks if group exists and if not throw exception
        await _groupRepository.DeleteByIdAsync(id);
    }
    
    public async Task<IEnumerable<Group>> GetByUserIdAsync(string userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        return await _groupRepository.FindAllAsync(g => g.UserId == user.Id);
    }

    public async Task<IEnumerable<Group>> GetByDisplayIdAsync(string displayId)
    {
        return await _groupRepository.FindAllAsync(g => g.Displays.Any(d => d.Id == displayId));
    }

    public async Task AddDisplayToGroupAsync(string groupId, string displayId)
    {
        var display = await _displayService.GetByIdAsync(displayId);
        var group = await GetByIdAsync(groupId);

        if (!group.Displays.Contains(display))
        {
            group.Displays.Add(display);
            await _groupRepository.UpdateAsync(group);
        }
        else
        {
            throw new InvalidOperationException("Display is already part of the group.");
        }
    }

    public async Task RemoveDisplayFromGroupAsync(string groupId, string displayId)
    {
        var display = await _displayService.GetByIdAsync(displayId);
        var group = await GetByIdAsync(groupId);

        if (!group.Displays.Contains(display))
            throw new InvalidOperationException("Display does not exist in the group.");

        group.Displays.Remove(display);
        await _groupRepository.UpdateAsync(group);
    }

    public async Task AddViewToGroupAsync(string groupId, string viewId)
    {
        var group = await GetByIdAsync(groupId)
            ?? throw new InvalidOperationException($"Group with id: {groupId} not found");
        
        var view = await _viewService.GetByIdAsync(viewId)
            ?? throw new InvalidOperationException($"View with id: {viewId} not found");

        if (view.GroupId != null)
            throw new InvalidOperationException($"View with id: {viewId} already belongs to a group");
        
        group.Views.Add(view);
    }

    public async Task RemoveViewFromGroupAsync(string groupId, string viewId)
    {
        var group = await GetByIdAsync(groupId);
        var view = await _viewService.GetByIdAsync(viewId);

        if (!group.Views.Contains(view))
            throw new InvalidOperationException($"View with id: {viewId} not found in group with id: {groupId}");
        
        view.Group = null;
    }

    public async Task<View?> GetCurrentViewAsync(string groupId)
    {
        var group = await GetByIdAsync(groupId);

        if (group?.Views.Count == 0)
            return null;

        var actualViews = group?.Views
            .Select(v => v?.To == null ? v : v.From < DateTime.Now && v.To > DateTime.Now ? v : null).ToList();
        return actualViews?.FirstOrDefault();
    }
}