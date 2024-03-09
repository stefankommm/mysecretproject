using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Entities.Display;
using Signee.ManagerWeb.Models.Display;
using Signee.Services.Areas.Display.Contracts;
using Signee.Resources.Resources;

namespace Signee.ManagerWeb.Controllers;

[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class DisplayController : ControllerBase
{
    private readonly IDisplayService _displayService;

    public DisplayController(IDisplayService displayService)
    {
        _displayService = displayService;
    }

    [Authorize (Roles = "Admin, User")]
    [HttpPost("")]
    public async Task<ActionResult<Display>> CreateDisplay([FromBody]CreateDisplayApi request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TODO add error to the service if we try to add display with name that already exists

            var display = CreateDisplayApi.ToDomainModel(request);
            await _displayService.AddAsync(display);
            return CreatedAtAction(nameof(GetDisplay), new { id = display.Id }, display);
        }
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = Resource.ManagerWeb_CreatingDisplayError });
        }
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<DisplayResponseApi>> GetDisplay(string id)
    {
        try
        {
            var display = await _displayService.GetByIdAsync(id);
            var response = DisplayResponseApi.FromDomainModel(display);
            return Ok(response);
        } catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = string.Format(Resource.ManagerWeb_DisplayIdRetreivalError, id) });
        }
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<DisplayResponseApi>>> GetAllDisplays()
    {
        try
        {
            var displays = await _displayService.GetAllAsync();
            var response = displays.Select(d => DisplayResponseApi.FromDomainModel(d));
            return Ok(response);
        }
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = Resource.ManagerWeb_DisplaysRetreivalError });
        }
    }
}