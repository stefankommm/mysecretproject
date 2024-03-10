using Signee.Domain.Entities.Widget;
using Signee.Domain.Logic.Widgets;

namespace Signee.Domain.Logic;

public class WidgetFactory
{
    public WidgetBase CreateWidget(Widget model)
    {
        switch (model.Type)
        {
            case "WeatherWidget":
                return new WeatherWidget();
            // Add cases for other widget types
            default:
                throw new ArgumentException("Unsupported widget type", nameof(model.Type));
        }
    }
}