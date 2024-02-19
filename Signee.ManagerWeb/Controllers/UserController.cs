using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Identity;
using Signee.Infrastructure.PostgreSql.Data;
using Signee.ManagerWeb.Models.Auth;
using Signee.Services.Areas.Auth.Contracts;
using Signee.Services.Areas.Auth.Services;

namespace Signee.ManagerWeb.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    // TODO create user repository/Service for managing user
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        ITokenService tokenService, ILogger<UserController> logger)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody]RegistrationRequestApi requestApi)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userManager.CreateAsync(
                new ApplicationUser
                    { UserName = requestApi.Username, Email = requestApi.Email, Role = requestApi.Role },
                requestApi.Password!
            );

            if (result.Succeeded)
            {
                requestApi.Password = string.Empty; // NOT STORING PWD IN MEMORY LONGER THAN NECESSARY
                return CreatedAtAction(nameof(Register), new { email = requestApi.Email, role = Role.User },
                    requestApi);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message }); // TODO Add some message from resources
        }
        
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseApi>> Authenticate([FromBody] AuthRequestApi requestApi)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managedUser = await _userManager.FindByEmailAsync(requestApi.Email!);

            if (managedUser == null)
                return BadRequest("User does not exist");

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, requestApi.Password!);

            if (!isPasswordValid)
                return BadRequest("Invalid login credentials");

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == requestApi.Email);

            if (userInDb == null)
                return Unauthorized();

            var jwtToken = _tokenService.GenerateToken(userInDb);

            return Ok(new AuthResponseApi
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = jwtToken,
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message }); // TODO Add some message from resources
        }
    }
}