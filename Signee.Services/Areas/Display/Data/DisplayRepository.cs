using Signee.Infrastructure.PostgreSql.Data;
using Signee.Services.Areas.Display.Contracts;

namespace Signee.Services.Areas.Display.Data;

public class DisplayRepository : PostgreSqlRepository<Domain.Entities.Display.Display>, IDisplayRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DisplayRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    // TODO Add some specific methods for accessing display data in the future
}