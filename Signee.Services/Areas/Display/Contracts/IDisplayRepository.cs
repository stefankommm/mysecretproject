using Signee.Infrastructure.PostgreSql.Contracts;

namespace Signee.Services.Areas.Display.Contracts;
using Display = Domain.Entities.Display.Display;

public interface IDisplayRepository : IGenericRepository<Display>
{
    // TODO Add some display specific methods
}