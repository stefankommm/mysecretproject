using System.ComponentModel.DataAnnotations;
using Signee.Domain.Identity;

namespace Signee.Services.Areas.Auth.Models;

public class RegistrationRequestApi
{
    [Required]
    public string? Email { get; set; }
    
    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? Password { get; set; }

    public Role Role { get; set; } = Role.User;
}