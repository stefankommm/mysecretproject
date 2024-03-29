using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Common;
using Signee.Domain.Identity;

namespace Signee.Domain.Entities.Group;
using Display = Signee.Domain.Entities.Display.Display;
using View = Signee.Domain.Entities.View.View;

public class Group : BaseEntity
{
    public string Name { get; set; }
    
    // Relationships
    public string? UserId { get; set; }
    
    // Virtual Relationships
    public ICollection<Display> Displays { get; set; } = new List<Display>();
    public ICollection<View> Views { get; set; } = new List<View>();
    
    public ApplicationUser? User { get; set; }

}
