using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signee.Domain.Entities.View;
using Group;
using Widget;

public class View
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = string.Empty;    
    public DateTime From { get; set; }  
    public DateTime To { get; set; }
    public string? GroupId { get; set; }
    public Group? Group { get; set; }
    
    public ICollection<Widget> Widgets { get; set; } = new List<Widget>();
}