using Microsoft.AspNetCore.Identity;
using Signee.Domain.Identity;

namespace Signee.Services.Areas.User.Contracts;

/// <summary>
/// Service contract for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Checks if user with ID exists and if it does retrieves user
    /// </summary>
    /// <param name="id">user ID</param>
    /// <returns>ApplicationUser with given ID</returns>
    Task<ApplicationUser> GetByIdAsync(string id);
    
    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to find.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    Task<ApplicationUser?> FindByEmailAsync(string email);

    /// <summary>
    /// Checks if an email address is unique in the system.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the email address is unique.</returns>
    Task<bool> IsEmailUniqueAsync(string email);

    /// <summary>
    /// Creates a new user with the specified user information and password.
    /// </summary>
    /// <param name="user">The user information for the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating whether the user creation was successful.</returns>
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

    /// <summary>
    /// Updates application user in DB
    /// </summary>
    /// <param name="user">ApplicationUser object to update</param>
    /// <returns></returns>
    Task UpdateAsync(ApplicationUser user);

    /// <summary>
    /// Checks if the specified password is valid for the given user.
    /// </summary>
    /// <param name="user">The user to check the password against.</param>
    /// <param name="password">The password to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the password is valid for the user.</returns>
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

    /// <summary>
    /// Ensures that an admin user exists in the system. If no admin user with the provided email exists, 
    /// one will be created using the specified email and password.
    /// </summary>
    /// <param name="adminEmail">The email of the admin user to be created.</param>
    /// <param name="adminPassword">The password of the admin user to be created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// If an admin user with the provided email already exists, this method does nothing.
    /// </remarks>
    Task EnsureAdminUserCreatedAsync(string adminEmail, string adminPassword);
}