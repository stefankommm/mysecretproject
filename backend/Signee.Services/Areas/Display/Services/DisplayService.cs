using System.Net;
using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Group.Contracts;
using Signee.Services.Areas.View.Contracts;

namespace Signee.Services.Areas.Display.Services;
using Display = Domain.Entities.Display.Display;
using View = Domain.Entities.View.View;

public class DisplayService : IDisplayService
{
    private readonly IDisplayRepository _displayRepository;
    private readonly IViewService _viewService;
    private readonly IGroupService _groupService;

    public DisplayService(IDisplayRepository displayRepository, IViewService viewService, IGroupService groupService)
    {
        _displayRepository = displayRepository;
        _viewService = viewService;
        _groupService = groupService;
    }
    //#TODO not complete yet 
    public async Task<bool> IsOnlineAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);
        if (display == null)
            throw new InvalidOperationException($"Display with id: {id} not found!");
        
        // #TODO Vymysliet ako ukladat Online, aby pri getovani displayov bola vzdy aktualna hodnota 
        return display?.LastOnline > DateTime.Now.AddMinutes(-60);
    }

    public async Task<string> GeneratePairingCodeAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id) 
                      ?? throw new InvalidOperationException($"Display with id: {id} not found!");
        
        display.PairingCode = Guid.NewGuid();
        await _displayRepository.UpdateAsync(display);
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
        
        if(display.GroupId == null) 
            throw new InvalidOperationException($"Display with id: {id} is not yet in any group!");
        
        // If the devices fetches this route -> service.. we know the display is online, its IP and ViewPort
        display.IpAddress = ipAddress;
        display.ViewPort = viewPort;
        display.LastOnline = DateTime.Now;
        await _displayRepository.UpdateAsync(display);
        
        return await _groupService.GetCurrentViewAsync(display?.GroupId!);
    }
    
    public async Task<IEnumerable<Display>> GetAllAsync() => await _displayRepository.GetAllAsync();

    public async Task<Display> GetByIdAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);
        if (display == null)
            throw new InvalidOperationException($"Display with id: {id} not found!");

        return display;
    }

    public async Task AddAsync(Display display)
    {
        if (display.Id != null)
            display.Id = null;

        // #IOI1 - V jednej Groupe nemozu byt dva displeje s identickym nazvom
        if(display.GroupId != null)
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
        
        // Validate if the Display exists ?not => throw exception
        var d = await GetByIdAsync(display.Id ?? string.Empty);
        if (d == null)
            throw new InvalidOperationException($"Display with id: {display.Id} not found => Cannot update display!");
        
        // Validate if the name changed || groupId changed ?yes => check if the new name is unique in the group
        if(display.GroupId != null)
            if (display.Name != d.Name || display.GroupId != d.GroupId)
            {
                var group = await _groupService.GetByIdAsync(display.GroupId);
                if (group.Displays.Any(x => x.Name == display.Name))
                    throw new InvalidOperationException($"Display with name: {display.Name} already exists in the group!");
            }
        
        await _displayRepository.UpdateAsync(display);
    } 

    public async Task DeleteByIdAsync(string id)
    {
        if(id == null)
            throw new InvalidOperationException("Display Id is null -> Cannot delete display!");
        
        var display = _displayRepository.GetByIdAsync(id);
        if(display == null)
            throw new InvalidOperationException($"Display with id: {id} not found => Cannot delete display!");
        
        await _displayRepository.DeleteByIdAsync(id);
    } 
}