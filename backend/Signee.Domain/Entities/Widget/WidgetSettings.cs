using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Common;

namespace Signee.Domain.Entities.Widget;

public class WidgetSettings : BaseEntity
{
    public bool required { get; set; } = false;  
    
    public string name { get; set; }
    
    public string type { get; set; }
    
    public string? value { get; set; }
    
    // Relationships
    public string WidgetId { get; set; }
    
    // Virtual Relationships
    public Widget Widget { get; set; }

}