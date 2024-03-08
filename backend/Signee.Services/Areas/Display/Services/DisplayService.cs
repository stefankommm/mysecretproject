using System.Net;
using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Areas.View.Contracts;
using Signee.Services.Auth.Contracts;

namespace Signee.Services.Areas.Display.Services;

using Display = Domain.Entities.Display.Display;
using View = Domain.Entities.View.View;

public class DisplayService : IDisplayService
{
    private readonly IDisplayRepository _displayRepository;
    private readonly IGroupService _groupService;
    private readonly IViewService _viewService;
    private readonly IUserContextProvider _userContextProvider;

    public DisplayService(IDisplayRepository displayRepository, IViewService viewService, IGroupService groupService, IUserContextProvider userContextProvider)
    {
        _displayRepository = displayRepository;
        _viewService = viewService;
        _groupService = groupService;
        _userContextProvider = userContextProvider;
    }

    //#TODO not complete yet 
    public async Task<bool> IsOnlineAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);

        if (!_userContextProvider.IsAdmin())
        {
            if(display == null) 
                throw new InvalidOperationException($"Display with id: {id} not found!");
            if (display.UserId != _userContextProvider.GetCurrentUserId())
                throw new InvalidOperationException("You are not the owner of this display!");
        }
        
        if (display == null)
            throw new InvalidOperationException($"Display with id: {id} not found!");
        
        return display?.LastOnline > DateTime.Now.AddMinutes(-60);
    }

    public async Task<string> RegeneratePairingCodeAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id)
                      ?? throw new InvalidOperationException($"Display with id: {id} not found!");
        if (!_userContextProvider.IsAdmin() && display.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this display!");
        
        display.PairingCode = Guid.NewGuid();
        await UpdateAsync(display);
        return display.PairingCode.ToString()!;
    }

    public async Task<View?> GetCurrentViewAsync(string id, bool fromDevice = false)
    {
        //TODO
        var display = await _displayRepository.GetByIdAsync(id);
        throw new NotImplementedException();
    }

    public async Task<View?> GetCurrentViewFromDeviceAsync(string id, IPAddress ipAddress, string viewPort)
    {
        var display = await _displayRepository.GetByIdAsync(id)
                      ?? throw new InvalidOperationException($"Display with id: {id} not found!");

        if (display.GroupId == null)
            throw new InvalidOperationException($"Display with id: {id} is not yet in any group!");

        // If the devices fetches this route -> service.. we know the display is online, its IP and ViewPort
        display.IpAddress = ipAddress;
        display.ViewPort = viewPort;
        display.LastOnline = DateTime.Now;
        await _displayRepository.UpdateAsync(display);

        return await _groupService.GetCurrentViewAsync(display?.GroupId!);
    }
    /// <summary>
    /// Gets all displays asynchronously.(User-Owned or Admin-All)
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Display>> GetAllAsync()
    {
        if (_userContextProvider.IsAdmin())
            return await _displayRepository.GetAllAsync();    
        
        return await _displayRepository.FindAllAsync(d => d.UserId == _userContextProvider.GetCurrentUserId());
    }

    public async Task<Display> GetByIdAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);
        if (display == null)
            throw new InvalidOperationException($"Display with id: {id} not found!");
        if (!_userContextProvider.IsAdmin() && display.UserId != _userContextProvider.GetCurrentUserId())
            throw new InvalidOperationException("You are not the owner of this display!");
        
        return display;
    }

    public async Task AddAsync(Display display)
    {
        if(display.GroupId == null)
            throw new InvalidOperationException("Display must be added also to a group!");
        if (display.Id != null)
            display.Id = null;
        
        display.UserId = _userContextProvider.GetCurrentUserId();
        
        // #IOI1 - V jednej Groupe nemozu byt dva displeje s identickym nazvom
        if (display.GroupId != null)
        {
            var group = await _groupService.GetByIdAsync(display.GroupId);
            if (group.Displays.Any(x => x.Name == display.Name))
                throw new InvalidOperationException($"Display with name: {display.Name} already exists in the group!");
        }

        await _displayRepository.AddAsync(display);
    }

    public async Task UpdateAsync(Display display)
    {
        // #IOI1 - V jednej Groupe nemozu byt dva displeje s identickym nazvom
        // If the display Id is null => throw exception
        if (display.Id == null)
            throw new InvalidOperationException("Display Id is null -> Cannot update display!");
        var d = await GetByIdAsync(display.Id ?? string.Empty);
        var groupOfTheDisplay = await _groupService.GetByIdAsync(d.GroupId) ?? null;
        var isAdmin = _userContextProvider.IsAdmin();
        var userId = _userContextProvider.GetCurrentUserId();
        
        // Validate if the Display exists ?not => throw exception
        if (d == null)
            throw new InvalidOperationException($"Display with id: {display.Id} not found => Cannot update display!");

        if (!isAdmin)
        {
            if(display.UserId != userId)
                throw new InvalidOperationException("You can't change the owner of the display!");
            // Validate if the name changed || groupId changed ?yes => check if the new name is unique in the group
            if (groupOfTheDisplay != null && (display.Name != d.Name || display.GroupId != d.GroupId))
                if (groupOfTheDisplay.Displays.Any(x => x.Name == display.Name))
                    throw new InvalidOperationException(
                        $"Display with name: {display.Name} already exists in the group!");
        }

        await _displayRepository.UpdateAsync(display);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var userId = _userContextProvider.GetCurrentUserId();
        var isAdmin = _userContextProvider.IsAdmin();
        
        if (id == null)
            throw new InvalidOperationException("Display Id is null -> Cannot delete display!");
        var display = await _displayRepository.GetByIdAsync(id) 
                      ?? throw new InvalidOperationException($"Display with id: {id} not found => Cannot delete display!");

        if (!isAdmin && display.UserId != userId)
            throw new InvalidOperationException("You are not the owner of this display!");
        
        await _displayRepository.DeleteByIdAsync(id);
    }
}