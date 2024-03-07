using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Signee.Domain.Identity;

namespace Signee.Domain.Entities.Display;

using Group = Group.Group;

public class Display
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    [MaxLength(20, ErrorMessage = "The Name field cannot exceed 20 characters.")]
    public string Name { get; set; } = string.Empty;
    
    public Guid? PairingCode { get; set; }
    
    public bool Online { get; set; } = false;
    
    public bool PairedWithDevice { get; set; } = false;
    
    public DateTime? LastOnline { get; set; }
    
    public IPAddress? IpAddress { get; set; }
    
    public string? ViewPort { get; set; }
    
    // Relationships
    public string? GroupId { get; set; }
    
    public string UserId { get; set; }

    // Virtual Relationships
    public Group? Group { get; set; }
    
    public ApplicationUser User { get; set; }
}