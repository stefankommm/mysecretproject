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

    public async Task<Display?> GetByIdAsync(string id) => await _displayRepository.GetByIdAsync(id);

    public async Task AddAsync(Display display) => await _displayRepository.AddAsync(display);

    public async Task UpdateByIdAsync(string id) => await _displayRepository.UpdateByIdAsync(id);
    public async Task UpdateAsync(Display display) => await _displayRepository.UpdateAsync(display);

    public async Task DeleteByIdAsync(string id) => await _displayRepository.DeleteByIdAsync(id);
}