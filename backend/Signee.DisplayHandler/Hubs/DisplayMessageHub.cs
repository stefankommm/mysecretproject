using Microsoft.AspNetCore.SignalR;
using Signee.DisplayHandler.Hubs.Contracts;
using Signee.Services.Areas.Display.Contracts;

namespace Signee.DisplayHandler.Hubs;

public sealed class DisplayMessageHub : Hub<IDisplayMessageClient>
{
    // Maps display ID (from DB) to SignalR (generated) connection ID
    private readonly Dictionary<string, HashSet<string>> _displayConnectionIdMapping = new();
    private readonly IDisplayService _displayService;

    public DisplayMessageHub(IDisplayService displayService)
    {
        _displayService = displayService;
    }
    
    /// <summary>
    /// Register (map) connection ID to display ID
    /// </summary>
    /// <param name="displayId"></param>
    private void RegisterClient(string displayId)
    {
        var connectionId = Context.ConnectionId;
        
        if (!_displayConnectionIdMapping.ContainsKey(displayId))
            _displayConnectionIdMapping[displayId] = new HashSet<string>();
        
        _displayConnectionIdMapping[displayId].Add(connectionId);
    }

    /// <summary>
    /// Remove display ID from mapping when a client disconnects
    /// </summary>
    /// <param name="connectionId"></param>
    private void UnregisterClient(string connectionId)
    {
        var displayId  = _displayConnectionIdMapping.FirstOrDefault(x => x.Value.Contains(connectionId)).Key;
        if (displayId != null)
            _displayConnectionIdMapping.Remove(displayId);
    }
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        
        var displayIdHttpContext = Context.GetHttpContext()?.Request.Query["id"];
        if (displayIdHttpContext is null)
            throw new ArgumentNullException();

        var displayId = displayIdHttpContext.Value.ToString();

        if (string.IsNullOrEmpty(displayId))
            throw new ArgumentException();
        
        // Register (map) connection ID upon connecting
        RegisterClient(displayId);
        
        // Send display URL to client
        var display = await _displayService.GetByIdAsync(displayId);
        // await Clients.Client(Context.ConnectionId).ReceiveMessage(display?.ImgUrl ?? string.Empty);
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Unregister (unmap) connection ID upon disconnecting
        UnregisterClient(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task ReceiveMessageFromClient(string message)
    {
        Console.WriteLine($"Message recieved from {Context.ConnectionId} - message: {message}");
        await Clients.All.ReceiveMessage($"{Context.ConnectionId} has sent: {message}");
    }
}