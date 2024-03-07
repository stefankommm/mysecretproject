namespace Signee.Services.Areas.View.Contracts;
using View = Domain.Entities.View.View;

public interface IViewService
{
    Task AddAsync(View view, string groupId);
    
    Task<IEnumerable<View>> GetAllByGroupIdAsync(string groupId);
    
    Task<View> GetByIdAsync(string id);
    
    Task UpdateAsync(View view);
    
    Task DeleteByIdAsync(string id);
    
    Task<View?> GetViewByTimeAsyncAndGroupId(DateTime time, string groupId);
}