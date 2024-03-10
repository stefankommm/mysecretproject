namespace Signee.Services.Areas.Widget.Contracts;
using Widget = Domain.Entities.Widget.Widget;

public interface IWidgetService
{
    // Creates a new widget based on the provided model
    Task CreateWidgetAsync(Widget model);

    // Updates an existing widget
    Task UpdateWidgetAsync(int widgetId, Widget model);

    // Fetches and returns widget data by widget ID
    Task<Widget> GetWidgetByIdAsync(int widgetId);

    // Deletes a widget by its ID
    Task DeleteWidgetAsync(int widgetId);

    // Fetches new data for a specific type of widget, e.g., weather information
    // and optionally sends updates through WebSocket or similar real-time communication
    Task FetchAndUpdateWidgetDataAsync(int widgetId);
}