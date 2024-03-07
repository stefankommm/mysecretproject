using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Signee.Domain.Identity;
using Signee.Domain.RepositoryContracts.Areas.User;
using Signee.Services.Auth.Contracts;
using Signee.Resources.Resources;
using Signee.Services.Areas.User.Contracts;

namespace Signee.Services.Areas.User.Services;

//#Todo: Add _userContextProvider and appropriate methods
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IUserContextProvider _userContext;
    
    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IUserContextProvider userContext)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userContext = userContext;
    }
 
    
    public Task<string> WhoAmIAsync()
    {
        var roles = _userContext.GetCurrentRoles();
        var role = roles.FirstOrDefault();
        if (role == Role.Admin) return Task.FromResult("Admin");
        if (role == Role.User) return Task.FromResult("User");
        
        return Task.FromResult("I don't know");
    }

    public async Task<ApplicationUser> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException($"User with id: {id} not found!");

        return user;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email) 
        => await _userRepository.FindByEmailAsync(email);

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var existingUser = await _userRepository.FindByEmailAsync(email);
        return existingUser == null;
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password) =>
        await _userRepository.CreateUserAsync(user, password);

    public async Task UpdateAsync(ApplicationUser user)
    {
        await GetByIdAsync(user.Id); // Checks if user exists and ig not throws exception
        await _userRepository.UpdateAsync(user);
    }
    
    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => await _userRepository.CheckPasswordAsync(user, password);

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