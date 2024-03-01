using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Display;

namespace Signee.Domain.Entities.Widget;
using View = Domain.Entities.View.View;
public class Widget
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = string.Empty;    
    public string? Name { get; set; }  
    public string? Description { get; set; }
    public string Type { get; set; }
    public View View { get; set; }
    public string? ViewId { get; set; }
    
    public ICollection<WidgetSettings>? WidgetSettings { get; set; } = new List<WidgetSettings>();
}