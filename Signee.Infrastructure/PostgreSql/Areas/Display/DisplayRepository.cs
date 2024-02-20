using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Infrastructure.PostgreSql.Common;

namespace Signee.Infrastructure.PostgreSql.Areas.Display;
using Display = Domain.Entities.Display.Display;

public class DisplayRepository : PostgreSqlRepository<Display>, IDisplayRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DisplayRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    // TODO Add some specific methods for accessing display data in the future
}