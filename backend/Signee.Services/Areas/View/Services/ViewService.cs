using Signee.Domain.RepositoryContracts.Areas.View;
using Signee.Services.Areas.View.Contracts;

namespace Signee.Services.Areas.View.Services;

public class ViewService : IViewService
{
    private readonly IViewRepository _viewRepository; 
    
    public ViewService(IViewRepository viewRepository)
    {
        _viewRepository = viewRepository;
    }

    public async Task<IEnumerable<Domain.Entities.View.View>> GetAllAsync() => await _viewRepository.GetAllAsync();

    public async Task<Domain.Entities.View.View?> GetByIdAsync(string id) => await _viewRepository.GetByIdAsync(id);

    public async Task AddAsync(Domain.Entities.View.View view) => await _viewRepository.AddAsync(view);

    public async Task UpdateByIdAsync(string id) => await _viewRepository.UpdateByIdAsync(id);

    public async Task DeleteByIdAsync(string id) => await _viewRepository.DeleteByIdAsync(id);

    public async Task<IEnumerable<Domain.Entities.View.View>> GetAllByGroupId(string groupId) => await _viewRepository.GetAllByGroupId(groupId);

    public async Task GetViewAtTimeAsync(DateTime time) => await _viewRepository.GetViewAtTime(time);
}