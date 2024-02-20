using Microsoft.AspNetCore.Identity;
using Signee.Domain.Identity;

namespace Signee.Domain.RepositoryContracts.Areas.User;

/// <summary>
/// Repository contract for user-related data access operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to find.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    Task<ApplicationUser?> FindByEmailAsync(string email);

    /// <summary>
    /// Creates a new user with the specified user information and password.
    /// </summary>
    /// <param name="user">The user information for the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating whether the user creation was successful.</returns>
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

    /// <summary>
    /// Checks if the specified password is valid for the given user.
    /// </summary>
    /// <param name="user">The user to check the password against.</param>
    /// <param name="password">The password to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the password is valid for the user.</returns>
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}