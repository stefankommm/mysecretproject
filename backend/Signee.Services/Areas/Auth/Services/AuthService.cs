using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Signee.Domain.Identity;
using Signee.Resources.Resources;
using Signee.Services.Areas.Auth.Contracts;
using Signee.Services.Areas.Auth.Models;
using Signee.Services.Areas.User.Contracts;

namespace Signee.Services.Areas.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserService userService, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<IdentityResult> RegisterAsync(RegistrationRequestApi registrationRequest)
    {
        var user = new ApplicationUser
        {
            UserName = registrationRequest.Username,
            Email = registrationRequest.Email,
            Role = registrationRequest.Role
        };

        var result = await _userService.CreateUserAsync(user, registrationRequest.Password ?? throw new ArgumentNullException());
        if (!result.Succeeded)
            _logger.LogError("Failed to create user.");
        
        return result;
    }

    public async Task<AuthResponseApi> AuthenticateAsync(AuthRequestApi authRequest)
    {
        var user = await _userService.FindByEmailAsync(authRequest.Email ?? throw new ArgumentNullException());
        if (user == null)
        {
            _logger.LogError("User does not exist.");
            throw new InvalidOperationException(message: Resource.Auth_InvalidLoginCredentials);
        }

        var isPasswordValid = await _userService.CheckPasswordAsync(user, authRequest.Password ?? throw new ArgumentNullException());
        if (!isPasswordValid)
        {
            _logger.LogError("Wrong password.");
            throw new InvalidOperationException(message: Resource.Auth_InvalidLoginCredentials);
        }

        var response = new AuthResponseApi
        {
            Username = user.UserName ?? throw new ArgumentNullException(),
            Email = user.Email,
            Token = _tokenService.GenerateToken(user)
        };

        return response;
    }
}