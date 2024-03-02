using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Domain.RepositoryContracts.Areas.User;
using Signee.Domain.RepositoryContracts.Areas.View;
using Signee.Services.Areas.Group.Contracts;

namespace Signee.Services.Areas.Group.Services;
using Group = Domain.Entities.Group.Group;
using View = Domain.Entities.View.View;
public class GroupService : IGroupService
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IDisplayRepository _displayRepository;
    private readonly IViewRepository _viewRepository;
    
    public GroupService(IUserRepository userRepostory ,IGroupRepository groupRepository, IDisplayRepository displayRepository, IViewRepository viewRepository)
    {
        _userRepository = userRepostory;
        _groupRepository = groupRepository;
        _displayRepository = displayRepository;
        _viewRepository = viewRepository;
    }
    

    public async Task<IEnumerable<Group>> GetAll()
    {
        return await _groupRepository.GetAll();
    }

    public async Task<Group?> GetById(string id)
    {
        var group = await _groupRepository.GetById(id);
        if (group == null)
            throw new Exception("Group not found");

        return group;
    }
    
    public async Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync()
    {
        var groups = await _groupRepository.FindAsync(g => (g.Displays.Count == 1));
        return groups;
    }

    public async Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync()
    {
        var groups = await _groupRepository.FindAsync(g => (g.Displays.Count > 1));
        return groups;
    }

    public async Task Update(Group g)
    {
        var group = await _groupRepository.GetById(g.Id);
        if(group == null)
            throw new Exception("Group not found");
        await _groupRepository.UpdateAsync(group);
    }

    public async Task DeleteById(string id)
    {
        var group = await _groupRepository.GetById(id);
        if(group == null)
            throw new Exception("Group not found");
        await _groupRepository.DeleteAsync(group);
    }

    public async Task Add(Group g, string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if(user == null)
            throw new Exception("User to attach display not found");
        g.User = user;

        await _groupRepository.AddAsync(g);
    }

    // User
    public async Task<IEnumerable<Group>> GetByUserId(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if(user == null){
            throw new Exception("Can't find user to find its groups");
        }

        var groups = await _groupRepository.FindAsync(g => g.UserId == userId);
        return groups;
    }

    //Display
    public async Task<IEnumerable<Group>> GetByDisplayId(string displayId)
    {
        var group = await _groupRepository.FindAsync(g => g.Displays.Any(d => d.Id == displayId));
        return group;
    }
    
    public async Task AddDisplayToGroup(string groupId, string displayId)
    { 
        var display = await _displayRepository.GetByIdAsync(displayId);
        var group = await _groupRepository.GetByIdAsync(groupId);
        
        if (display == null || group == null)
            throw new Exception("Display or group not found");
        
        group.Displays.Add(display);
        
        await _groupRepository.UpdateAsync(group);
    }

    public async Task DeleteDisplayFromGroup(string groupId, string displayId)
    {
        var display = await _displayRepository.GetByIdAsync(displayId);
        var group = await _groupRepository.GetByIdAsync(groupId);
        
        if (display == null || group == null)
            throw new Exception("Display or group not found");
        
        display.Group = null;
        group.Displays.Remove(display);
        
        await _displayRepository.UpdateAsync(display);
        await _groupRepository.UpdateAsync(group);
    }
    
    //View
    public async Task AddViewToGroup(string groupId, string viewId)
    {
        var group = await _groupRepository.GetById(groupId);
        var view = await _viewRepository.GetById(viewId);
        if(group == null || view == null)
            throw new Exception("Group or view not found");
        
        group.Views.Add(view);
    }

    public async Task DeleteViewFromGroup(string groupId, string viewId)
    {
        var group = await _groupRepository.GetById(groupId);
        var view = await _viewRepository.GetById(viewId);
        if(group == null || view == null)
            throw new Exception("Group or view not found");
        if (!group.Views.Contains(view))
        {
            throw new Exception("View not found in group");
        }
        group.Views.Remove(view);
    }

    public async Task<View> GetCurrentView(string groupId)
    {
        var group = await _groupRepository.GetById(groupId);
        var views = group.Views.FirstOrDefault();
        if(group == null)
            throw new Exception("Group not found");
        if (group.Views.Count == 0)
            return null;
        var actualViews = group.Views.Select(v => v.To == null ? v : (v.From < DateTime.Now && v.To > DateTime.Now) ? v : null).ToList();
        return actualViews.FirstOrDefault();
    }
    
}