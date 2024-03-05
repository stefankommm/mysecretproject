using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.View;
using View = Domain.Entities.View.View;

public interface IViewRepository : IGenericRepository<View>
{
    Task<View?> GetViewByTimeAsync(DateTime time);
    
    Task<IEnumerable<View>> GetAllByGroupIdAsync(string groupId);
}