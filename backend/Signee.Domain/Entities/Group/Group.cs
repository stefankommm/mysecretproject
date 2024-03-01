using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Display;
using Signee.Domain.Identity;

namespace Signee.Domain.Entities.Group;
using Display = Signee.Domain.Entities.Display.Display;
using View = Signee.Domain.Entities.Display.View;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id{ get; set; }
    
    public string? Name { get; set; }
    public ICollection<Display> Displays { get; set; } = new List<Display>();
    public ICollection<View> Views { get; set; } = new List<View>();

    
}
