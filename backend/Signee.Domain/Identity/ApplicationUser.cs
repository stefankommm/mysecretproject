using Microsoft.AspNetCore.Identity;
using Signee.Domain.Entities.Display;
using Signee.Domain.Entities.Group;

namespace Signee.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}