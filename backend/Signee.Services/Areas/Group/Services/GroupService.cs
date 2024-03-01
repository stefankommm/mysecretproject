using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Services.Areas.Group.Contracts;

namespace Signee.Services.Areas.Group.Services;
using Group = Domain.Entities.Group.Group;
public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IDisplayRepository _displayRepository;
    
    public GroupService(IGroupRepository groupRepository, IDisplayRepository displayRepository)
    {
        _groupRepository = groupRepository;
        _displayRepository = displayRepository;
    }

    public async Task<IEnumerable<Group>> GetAllGroups()
    {
        var all = await _groupRepository.GetAll();
        return all;
    }

    public async Task<Group?> GetGroupById(string id)
    {
        var group = await _groupRepository.GetById(id);
        var displays = await _displayRepository.GetByGroupId(id);
        
        var result = new Group()
        {
            Id = group.Id,
            Name = group.Name,
            Displays = displays.ToList(),
            Views = group.Views
        };
        
        
        throw new NotImplementedException();
    }

    public async Task AddToGroup(Group group)
    {
        await _groupRepository.AddAsync(group);
    }

    public Task UpdateGroup(Group id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateGroup(Group group)
    {
        await _groupRepository.AddAsync(group);
    }

    public Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> GetByDisplayId(string displayId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> GetByUserId(string userId)
    {
        throw new NotImplementedException();
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

    public async Task RemoveDisplayFromGroup(string groupId, string displayId)
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
    
}