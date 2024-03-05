using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signee.Domain.Entities.Widget;

public class WidgetSettings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = string.Empty;
    public bool required { get; set; } = false;  
    public string name { get; set; }
    public string type { get; set; }
    public string? value { get; set; }
    
    public Widget Widget { get; set; }
    public string? WidgetId { get; set; }
}