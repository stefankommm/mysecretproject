namespace Signee.Services.Areas.Display.Contracts;

using Display = Domain.Entities.Display.Display;

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
    Task<Display?> GetByIdAsync(string id);

    /// <summary>
    /// Adds a new display asynchronously.
    /// </summary>
    /// <param name="display">The display to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(Display display);

    Task UpdateAsync(Display display);

    /// <summary>
    /// Deletes a display by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the display to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteByIdAsync(string id);
}