using Signee.Domain.RepositoryContracts.Areas.View;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Areas.View.Contracts;
using Signee.Services.Auth.Contracts;

namespace Signee.Services.Areas.View.Services;
using View = Domain.Entities.View.View;

public class ViewService : IViewService
{
    private readonly IViewRepository _viewRepository;
    private readonly IGroupService _groupService;
    private readonly IUserContextProvider _userContextProvider;

    public ViewService(IViewRepository viewRepository, IGroupService groupService, IUserContextProvider userContextProvider)
    {
        _viewRepository = viewRepository;
        _groupService = groupService;
        _userContextProvider = userContextProvider;
    }
    
    public async Task AddAsync(View view, string groupId)
    {
        var group = await _groupService.GetByIdAsync(groupId);
        if(!_userContextProvider.isAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        await _viewRepository.AddAsync(view);
    }

    public async Task<IEnumerable<View>> GetAllByGroupIdAsync(string groupId)
    {
        var group = await _groupService.GetByIdAsync(groupId);
        if(!_userContextProvider.isAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        return await _viewRepository.GetAllByGroupIdAsync(groupId);
    }

    public async Task<View> GetByIdAsync(string id)
    {
        var view = await _viewRepository.GetByIdAsync(id)
                   ?? throw new InvalidOperationException($"View with id: {id} not found");
        var group = await _groupService.GetByIdAsync(view.GroupId);
        if(!_userContextProvider.isAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        return view;
    }

    public async Task UpdateAsync(View view)
    {
        var viewFromDb = await _viewRepository.GetByIdAsync(view.Id)
                         ?? throw new InvalidOperationException($"View with id: {view.Id} not found");
        var group = await _groupService.GetByIdAsync(view.GroupId);
        if(!_userContextProvider.isAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        await _viewRepository.UpdateAsync(view);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var view = await _viewRepository.GetByIdAsync(id)
                   ?? throw new InvalidOperationException($"View with id: {id} not found");
        var group = await _groupService.GetByIdAsync(view.GroupId);
        if(!_userContextProvider.isAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");
        
        await _viewRepository.DeleteByIdAsync(id);
    }

    public async Task<View?> GetViewByTimeAsyncAndGroupId(DateTime time, string groupId)
    {
        //#TODO aj pre fetchovanie z displeja
        var group = await _groupService.GetByIdAsync(groupId);
        if(!_userContextProvider.isAdmin() && group.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this group");

        return await _groupService.GetCurrentViewAsync(groupId);
    }
}