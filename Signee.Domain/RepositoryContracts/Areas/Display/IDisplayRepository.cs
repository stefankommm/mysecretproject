using Signee.Domain.RepositoryContracts.Common;

namespace Signee.Domain.RepositoryContracts.Areas.Display;
using Display = Domain.Entities.Display.Display;

public interface IDisplayRepository : IGenericRepository<Display>
{
    // TODO Add some display specific methods
}