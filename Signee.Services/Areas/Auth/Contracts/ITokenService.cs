using Signee.Domain.Identity;

namespace Signee.Services.Areas.Auth.Contracts;

public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is created.</param>
    /// <returns>The JWT token string.</returns>
    string GenerateToken(ApplicationUser user);
}