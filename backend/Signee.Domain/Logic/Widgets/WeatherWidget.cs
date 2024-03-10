using Signee.Domain.Entities.Widget;
using Signee.Domain.Entities.WidgetSettings;
using Signee.Domain.Logic;

namespace Signee.Domain.Logic.Widgets;


public class WeatherWidget : WidgetBase
{
    private string? City { get; set; }
    private string? Coordinates { get; set; }
    
    public WeatherWidget()
    {
        this.Type = WidgetTypes.WeatherWidget;
        this.Name = "Weather Widget";
        this.Description = "A widget to display weather information";
    }
    public override void Initialize(Widget model)
    {
        var widgetSettings = model.WidgetSettings
            ?? throw new ArgumentNullException(nameof(model.WidgetSettings));
        City = widgetSettings.FirstOrDefault(x => x.Name == "CITY_NAME")?.Value;
        Coordinates = widgetSettings.FirstOrDefault(x => x.Name == "COORDINATES")?.Value;
        Validate();
    }

    public override bool Validate()
    {
        if (City == null && Coordinates == null)
            throw new ArgumentException("City or Coordinates must be provided");
        if(City != null && Coordinates != null) 
            throw new ArgumentException("City and Coordinates cannot both be provided");
        
        // Validate City or Coordinates
        return true;
    }
    
    public override Widget ToEntity()
    {
        var widgetEntity = new Widget
        {
            Name = this.Name,
            Description = this.Description,
            Type = "WeatherWidget", // Assuming 'Type' is a discriminator or similar
            WidgetSettings = new List<WidgetSettings>()
        };
        
        if(City != null)
            widgetEntity.WidgetSettings.Add(new WidgetSettings { Name = "CITY_NAME", Value = City, Type = "string", Widget = widgetEntity });
        else
            widgetEntity.WidgetSettings.Add(new WidgetSettings { Name = "COORDINATES", Value = Coordinates, Type = "string", Widget = widgetEntity });
        
        return widgetEntity;
    }
}