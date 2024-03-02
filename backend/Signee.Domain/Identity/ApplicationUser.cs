using Microsoft.AspNetCore.Identity;
using Group = Signee.Domain.Entities.Group.Group;

namespace Signee.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
    public ICollection<Group> Groups { get; set; } = new List<Group>();
}