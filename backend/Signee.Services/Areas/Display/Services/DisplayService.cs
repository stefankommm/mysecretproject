using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Services.Areas.Display.Contracts;

namespace Signee.Services.Areas.Display.Services;
using Display = Domain.Entities.Display.Display;

public class DisplayService : IDisplayService
{
    private readonly IDisplayRepository _displayRepository;

    public DisplayService(IDisplayRepository displayRepository)
    {
        _displayRepository = displayRepository;
    }
    
    public async Task<IEnumerable<Display>> GetAllAsync() => await _displayRepository.GetAllAsync();

    public async Task<Display> GetByIdAsync(string id)
    {
        var display = await _displayRepository.GetByIdAsync(id);
        if (display == null)
            throw new InvalidOperationException($"Display with id: {id} not found!");

        return display;
    }

    public async Task AddAsync(Display display) => await _displayRepository.AddAsync(display);

    public async Task UpdateAsync(Display display)
    {
        await GetByIdAsync(display.Id ?? string.Empty); // Checks if display exists and if not throw exception
        await _displayRepository.UpdateAsync(display);
    } 

    public async Task DeleteByIdAsync(string id)
    {
        await GetByIdAsync(id); // Checks if display exists and if not throw exception
        await _displayRepository.DeleteByIdAsync(id);
    } 
}