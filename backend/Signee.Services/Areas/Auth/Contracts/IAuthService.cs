using Microsoft.AspNetCore.Identity;
using Signee.Services.Areas.Auth.Models;

namespace Signee.Services.Areas.Auth.Contracts;

/// <summary>
/// Service contract for user authentication and registration.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided registration information.
    /// </summary>
    /// <param name="registrationRequest">The registration information of the user.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating whether the registration was successful. (if not contains errors)</returns>
    Task<IdentityResult> RegisterAsync(RegistrationRequestApi registrationRequest);

    /// <summary>
    /// Authenticates a user with the provided authentication request.
    /// </summary>
    /// <param name="authRequest">The authentication request containing user credentials.</param>
    /// <returns>An <see cref="AuthResponseApi"/> containing authentication information (with jwt token) upon successful authentication.</returns>
    Task<AuthResponseApi> AuthenticateAsync(AuthRequestApi authRequest);
}