using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Common;

namespace Signee.Domain.Entities.WidgetSettings;
using Widget = Widget.Widget;
public class WidgetSettings : BaseEntity
{
    public bool Required { get; set; } = false;  
    
    public string Name { get; set; }
    
    public string Type { get; set; }
    
    public string? Value { get; set; }
    
    // Relationships
    public string WidgetId { get; set; }
    
    // Virtual Relationships
    public Widget Widget { get; set; }

}