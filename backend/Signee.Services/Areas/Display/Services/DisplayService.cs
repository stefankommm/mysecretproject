using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Services.Areas.Auth.Contracts;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Areas.View.Contracts;

namespace Signee.Services.Areas.Display.Services;
using Display = Domain.Entities.Display.Display;
using View = Domain.Entities.View.View;

public class DisplayService : IDisplayService
{
    private readonly IDisplayRepository _displayRepository;
    private readonly IGroupService _groupService;
    private readonly IViewService _viewService;
    private readonly IUserContextProvider _userContextProvider;
    
    // display<-group->views   views->widgety 

    public DisplayService(IDisplayRepository displayRepository, IGroupService groupService, IViewService viewService, IUserContextProvider userContextProvider)
    {
        _displayRepository = displayRepository;
        _groupService = groupService;
        _viewService = viewService;
        _userContextProvider = userContextProvider;
    }

    //#TODO not complete yet 
    // public async Task<bool> IsOnlineAsync(string id)
    // {
    //     var display = await _displayRepository.GetByIdAsync(id);
    //
    //     if (!_userContextProvider.isAdmin())
    //     {
    //         if (display.UserId != _userContextProvider.GetCurrentUserId())
    //             throw new InvalidOperationException("You are not the owner of this display!");
    //     }
    //     
    //     return display?.LastOnline > DateTime.Now.AddMinutes(-60);
    // }

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

    public async Task<Display> RegeneratePairingCodeAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);
        CheckDisplayOwnership(display);
        
        display.PairingCode = Guid.NewGuid();
        await UpdateAsync(display);
        
        return display;
    }
    
    public async Task<View?> GetCurrentViewFromDeviceAsync(string pairingCode)
    {
        var display = await _displayRepository.GetDisplayByPairingCodeAsync(new Guid(pairingCode));

        if (display.GroupId == null)
            throw new InvalidOperationException($"Display with pairing code {pairingCode} is not yet in any group!");

        // If the devices fetches this route -> service.. we know the display is online, its IP and ViewPort
        display.IpAddress = _userContextProvider.GetIpAddress();
        display.ViewPort = _userContextProvider.GetDeviceViewport();
        display.LastOnline = DateTime.Now;
        await _displayRepository.UpdateAsync(display);

        return await _groupService.GetCurrentViewAsync(display?.GroupId!);
    }
    
    /// <summary>
    /// Gets all displays asynchronously.(User-Owned or Admin-All)
    /// </summary>
    public async Task<IEnumerable<Display>> GetAllAsync()
    {
        if (_userContextProvider.isAdmin())
            return await _displayRepository.GetAllAsync();    
        
        return await _displayRepository.FindAllAsync(d => d.UserId == _userContextProvider.GetCurrentUserId());
    }

    public async Task<Display> GetByIdAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);
        CheckDisplayOwnership(display);
        
        return display;
    }

    public async Task AddAsync(Display display)
    {
        display.Id = string.Empty;
        display.UserId = _userContextProvider.GetCurrentUserId();
        
        // #IOI1 - Displays within one group must have unique names
        if (display.GroupId != null)
        {
            var group = await _groupRepository.GetByIdAsync(display.GroupId);
            if (group.Displays.Any(x => x.Name == display.Name))
                throw new InvalidOperationException($"Display with name: {display.Name} already exists in the group!");
        }

        await _displayRepository.AddAsync(display);
    }

    public async Task UpdateAsync(Display updatedDisplay)
    {
        // #IOI1 - V jednej Groupe nemozu byt dva displeje s identickym nazvom

        var currentDisplay = await _displayRepository.GetByIdAsync(updatedDisplay.Id);
        CheckDisplayOwnership(currentDisplay);

        
        var currentDisplayGroup = currentDisplay.Group;
        
        
        // Check authorization constrains
        var isAdmin = _userContextProvider.isAdmin();
        var userId = _userContextProvider.GetCurrentUserId();

        if (!isAdmin)
        {
            if(updatedDisplay.UserId != userId)
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
        var display = await _displayRepository.GetByIdAsync(id);
        CheckDisplayOwnership(display);
        
        await _displayRepository.DeleteByIdAsync(id);
    }
}