namespace Signee.Services.Areas.View.Contracts;
using View = Domain.Entities.View.View;

public interface IViewService
{
    Task<IEnumerable<View>> GetAllAsync();
    
    Task<View> GetByIdAsync(string id);
    
    Task AddAsync(View view);
    
    Task UpdateAsync(View view);
    
    Task DeleteByIdAsync(string id);
    
    Task<IEnumerable<View>> GetAllByGroupIdAsync(string groupId); // if group name unique -> could use name
    
    Task GetViewByTimeAsync(DateTime time);
}