using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Identity;
using Signee.Resources.Resources;
using Signee.Services.Areas.Auth.Contracts;
using Signee.Services.Areas.Auth.Models;

namespace Signee.ManagerWeb.Controllers;

[ApiVersion( 1.0 )]
[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;

    public AuthController(IAuthService authenticationService)
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
            return Ok(new { message = Resource.Auth_LoginSuccessfull, userData = authResponse });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}