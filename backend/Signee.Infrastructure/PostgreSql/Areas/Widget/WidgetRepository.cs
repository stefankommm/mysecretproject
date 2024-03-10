using Signee.Domain.RepositoryContracts.Areas.Widget;
using Signee.Infrastructure.PostgreSql.Common;

namespace Signee.Infrastructure.PostgreSql.Areas.Widget;
using Widget = Domain.Entities.Widget.Widget;

public class WidgetRepository : PostgreSqlRepository<Widget> , IWidgetRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WidgetRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    
}

