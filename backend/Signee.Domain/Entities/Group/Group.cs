using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Display;
using Signee.Domain.Identity;

namespace Signee.Domain.Entities.Group;
using Display = Signee.Domain.Entities.Display.Display;
using View = Signee.Domain.Entities.View.View;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id{ get; set; }
    
    public string? Name { get; set; }

    public bool? IsGroup => Displays?.Count > 1; // The Group is treated as a SingleDisplay if it contains one Display
    public ICollection<Display>? Displays { get; set; }  = new List<Display>();
    
    public ICollection<View>? Views { get; set; }  = new List<View>();
}
