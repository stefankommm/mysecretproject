using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Identity;
using Signee.Services.Areas.User.Contracts;

namespace Signee.ManagerWeb.Controllers;

[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpPost("")]
    public async Task<ActionResult<string>> WhoAmI()
    {
        return await _userService.WhoAmIAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
    {
       
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
    }
    
}