using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Services.Areas.Group.Contracts;

namespace Signee.Services.Areas.Group.Services;
using Group = Domain.Entities.Group.Group;
public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository; 
    
    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }


    public async Task<IEnumerable<Group>> GetAllAsync() => await _groupRepository.GetAllAsync();

    public async Task<Group?> GetByIdAsync(string id) => await _groupRepository.GetByIdAsync(id);

    public async Task AddAsync(Group group) => await _groupRepository.AddAsync(group);

    public async Task UpdateByIdAsync(string id) => await _groupRepository.UpdateByIdAsync(id);

    public async Task DeleteByIdAsync(string id) => await _groupRepository.DeleteByIdAsync(id);

    public async Task<IEnumerable<Group>> GetAllWithSingleDisplayAsync() => await _groupRepository.GetAllWithSingleDisplay();

    public async Task<IEnumerable<Group>> GetAllWithMultipleDisplaysAsync() => await _groupRepository.GetAllWithMultipleDisplays();

    public Task<IEnumerable<Group>> GetByDisplayIdAsync(string displayId) => throw new  NotImplementedException();

    public async Task<IEnumerable<Group>> GetByUserIdAsync(string userId) => throw new  NotImplementedException();
}