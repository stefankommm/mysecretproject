using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Signee.Domain.Identity;
using Signee.Domain.RepositoryContracts.Areas.User;
using Signee.Resources.Resources;
using Signee.Services.Areas.User.Contracts;

namespace Signee.Services.Areas.User.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userRepository.FindByEmailAsync(email);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var existingUser = await _userRepository.FindByEmailAsync(email);
        return existingUser == null;
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        return await _userRepository.CreateUserAsync(user, password);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userRepository.CheckPasswordAsync(user, password);
    }

    public async Task EnsureAdminUserCreatedAsync(string adminEmail, string adminPassword)
    {
        var adminUser = await _userRepository.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
            var result = await _userRepository.CreateUserAsync(newUser, adminPassword);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create default admin user");
                throw new InvalidOperationException(message: Resource.PopulateDb_AdminUserCreationError);
            }
        }
    }
}