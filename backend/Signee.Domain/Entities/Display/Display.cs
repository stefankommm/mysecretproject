using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Identity;

namespace Signee.Domain.Entities.Display;
using Group = Group.Group;
public class Display 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; } // "www.nasastranka.com/display/Id=123"
    
    [MaxLength(20, ErrorMessage = "The Name field cannot exceed 20 characters.")]
    public string? Name { get; set; } = string.Empty;
    
    public int? PairingCode { get; set; }
    
    public Group? Group { get; set; }
    
    public string? GroupId { get; set; }
}
