using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Signee.Domain.Identity;

namespace Signee.Domain.Entities.Display;
    
public class Display 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    
    [MaxLength(20, ErrorMessage = "The Name field cannot exceed 20 characters.")]
    public string? Name { get; set; } = string.Empty;
    
    public string? Url { get; set; } = string.Empty;
    
    public int? PairingCode { get; set; }
    
    public Content? Content { get; set; }
    
    // TODO uncomment when userRepository/Service is ready
    // [ForeignKey("CreatedBy")]
    // public string CreatedById { get; set; } = string.Empty;
    // public ApplicationUser CreatedBy { get; set; }
}

// Display (ULR) <- Content (from-to) <- IEnumerable<Widget> (Layout)