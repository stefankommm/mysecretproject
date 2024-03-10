using Signee.Infrastructure.PostgreSql.Common;

namespace Signee.Infrastructure.PostgreSql.Areas.WidgetSettings;
using WidgetSettings = Domain.Entities.WidgetSettings.WidgetSettings;

public class WidgetSettingsRepositrory : PostgreSqlRepository<WidgetSettings> // IWidgetRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WidgetSettingsRepositrory(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}