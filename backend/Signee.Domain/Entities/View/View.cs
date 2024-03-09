using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Entities.Common;

namespace Signee.Domain.Entities.View;
using Group;
using Widget;

public class View : BaseEntity
{
    public string Name { get; set; }
    public DateTime? From { get; set; }
    
    public DateTime? To { get; set; }
    
    //Relationships
    public string GroupId { get; set; }
    
    // Virtual Realtionships
    public Group Group { get; set; }
    
    public ICollection<Widget> Widgets { get; set; } = new List<Widget>();
}