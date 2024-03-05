using Signee.Domain.RepositoryContracts.Areas.View;
using Signee.Services.Areas.View.Contracts;

namespace Signee.Services.Areas.View.Services;
using View = Domain.Entities.View.View;

public class ViewService : IViewService
{
    private readonly IViewRepository _viewRepository;

    public ViewService(IViewRepository viewRepository)
    {
        _viewRepository = viewRepository;
    }

    public async Task<IEnumerable<View>> GetAllAsync() => await _viewRepository.GetAllAsync();

    public async Task<View> GetByIdAsync(string id)
    {
        var view = await _viewRepository.GetByIdAsync(id);
        if (view == null)
            throw new InvalidOperationException($"View with id: {id} not found!");

        return view;
    } 
    
    public async Task AddAsync(View view) => await _viewRepository.AddAsync(view);

    public async Task UpdateAsync(View view) => await _viewRepository.UpdateAsync(view);

    public async Task DeleteByIdAsync(string id)
    {
        await GetByIdAsync(id); // Checks if view exists and if not throw exception
        await _viewRepository.DeleteByIdAsync(id);
    } 

    public async Task<IEnumerable<View>> GetAllByGroupIdAsync(string groupId) 
        => await _viewRepository.GetAllByGroupIdAsync(groupId);

    public async Task GetViewByTimeAsync(DateTime time) => await _viewRepository.GetViewByTimeAsync(time);
}