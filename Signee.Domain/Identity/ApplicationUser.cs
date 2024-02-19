using Microsoft.AspNetCore.Identity;
using Signee.Domain.Entities.Display;

namespace Signee.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
    public ICollection<Display> CreatedDisplays { get; set; }
}