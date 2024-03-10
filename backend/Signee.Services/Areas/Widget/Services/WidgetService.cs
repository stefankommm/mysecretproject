using Signee.Domain.Logic;
using Signee.Domain.RepositoryContracts.Areas.Widget;
using Signee.Domain.RepositoryContracts.Areas.WidgetSettings;
using Signee.Services.Areas.Widget.Contracts;

namespace Signee.Services.Areas.Widget.Services;
using Widget = Domain.Entities.Widget.Widget;

public class WidgetService : IWidgetService
{
    private readonly WidgetFactory _widgetFactory = new WidgetFactory();
    private readonly IWidgetRepository _widgetRepository;
    private readonly IWidgetSettingsRepository _widgetSettingsRepository;
    
    public WidgetService(IWidgetRepository widgetRepository, IWidgetSettingsRepository widgetSettingsRepository)
    {
        _widgetRepository = widgetRepository;
        _widgetSettingsRepository = widgetSettingsRepository;
    }
    
    public async Task CreateWidgetAsync(Widget model){
    
        WidgetBase widgetBase = _widgetFactory.CreateWidget(model);
        widgetBase.Initialize(model);
        
        var dbEntity = widgetBase.ToEntity();
        await _widgetRepository.AddAsync(dbEntity);
        
        throw new NotImplementedException();
    }

    public async Task UpdateWidgetAsync(int widgetId, Widget model)
    {
        var w = _widgetRepository.GetByIdAsync(widgetId)
                ?? throw new InvalidOperationException($"Widget with id: {widgetId} not found");
        var widget = _widgetFactory.CreateWidget(model);
        widget.Initialize(model);
        var dbEntity = widget.ToEntity();
        dbEntity.Id = model.Id;

        foreach (var setting in dbEntity.WidgetSettings)
            await _widgetSettingsRepository.UpdateAsync(setting);
        
        await _widgetRepository.UpdateAsync(dbEntity);
        
    }

    public async Task<Widget> GetWidgetByIdAsync(int widgetId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteWidgetAsync(int widgetId)
    {
        throw new NotImplementedException();
    }

    public async Task FetchAndUpdateWidgetDataAsync(int widgetId)
    {
        throw new NotImplementedException();
    }
}