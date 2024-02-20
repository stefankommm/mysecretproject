using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Identity;
using Signee.ManagerWeb.Models.Auth;
using Signee.Services.Areas.Auth.Contracts;

namespace Signee.ManagerWeb.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthService _authenticationService;

    public UserController(IAuthService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody]RegistrationRequestApi requestApi)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.RegisterAsync(requestApi);

            if (result.Succeeded)
            {
                requestApi.Password = string.Empty; // NOT STORING PWD IN MEMORY LONGER THAN NECESSARY
                return CreatedAtAction(nameof(Register), new { email = requestApi.Email, role = Role.User }, requestApi);
            }
            
            // TODO use resources to map errors to resource and return localized text
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
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

            var authResponse = await _authenticationService.AuthenticateAsync(requestApi);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}