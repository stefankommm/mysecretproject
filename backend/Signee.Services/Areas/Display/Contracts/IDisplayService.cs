using System.Net;

namespace Signee.Services.Areas.Display.Contracts;
using Display = Domain.Entities.Display.Display;
using View = Domain.Entities.View.View;
public interface IDisplayService
{
    /// <summary>
    /// Retrieves all displays asynchronously.
    /// </summary>
    /// <returns>A collection of displays.</returns>
    Task<IEnumerable<Display>> GetAllAsync();

    /// <summary>
    /// Retrieves a display by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the display to retrieve.</param>
    /// <returns>The display with the specified identifier.</returns>
    Task<Display> GetByIdAsync(string id);

    /// <summary>
    /// Adds a new display asynchronously.
    /// </summary>
    /// <param name="display">The display to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(Display display);
    
    /// <summary>
    /// Updates a display asynchronously.
    /// </summary>
    /// <param name="display">The display object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Display display);

    /// <summary>
    /// Deletes a display by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the display to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteByIdAsync(string id);
    
    
    /// <summary>
    /// Gets display status
    /// </summary>
    /// <param name="id">ID of the given display</param>
    /// <returns>True if Online(last <60minutes)</returns>
    Task<bool> IsOnlineAsync(string id);
    
    /// <summary>
    /// Generates new Parining code for the given display asynchronously.
    /// <param name="id">Id of the given display</param>
    /// <returns>GUID Pairing code</returns>
    Task<Display> RegeneratePairingCodeAsync(string id);
    
    /// <summary>
    /// Gets current view of the display asynchronously.
    /// <param name="id">Id of the given display</param>
    /// <returns>Current View of the display</returns>
    Task<View?> GetCurrentViewAsync(string id, bool fromDevice = false);
    
    /// <summary>
    /// Gets current view of the display asynchronously.
    /// Used only by devices
    /// <param name="pairingCode">Id of the given display</param>
    /// <param name="ipAddress">IP address of the device calling the request</param>
    /// <param name="viewPort">ViewPort of the device calling the request</param>
    Task<View?> GetCurrentViewFromDeviceAsync(string pairingCode);
    
    
}