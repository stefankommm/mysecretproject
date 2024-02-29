using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.View;
using View = Domain.Entities.View.View;

public interface IViewRepository : IGenericRepository<Entities.View.View>
{
    Task <IEnumerable<View>> GetAll();
    
    Task<View?> GetById(string viewId);
    
    Task<View?> GetViewAtTime(DateTime time);
    
    Task<IEnumerable<View>>  GetAllByGroupId(string groupId);
}