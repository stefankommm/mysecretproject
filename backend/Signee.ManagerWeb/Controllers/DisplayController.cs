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
    [HttpPost("createDisplay")]
    public async Task<ActionResult<Display>> CreateDisplay([FromBody]CreateDisplayApi requestCreateDisplay)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TODO add error to the service if we try to add display with name that already exists

            var display = new Display
            {
                Name = requestCreateDisplay.Name,
                ImgUrl = requestCreateDisplay.ImgUrl
            };
            
            await _displayService.AddAsync(display);
            return CreatedAtAction(nameof(GetDisplay), new { id = display.Id }, display);
        }
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = Resource.ManagerWeb_ErrorCreatingDisplay });
        }
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpGet("id")]
    public async Task<ActionResult<DisplayApi>> GetDisplay([FromQuery(Name = "id")] string id)
    {
        try
        {
            var display = await _displayService.GetByIdAsync(id);

            if (display == null)
                return NotFound();
        
            var displayApi = new DisplayApi
            {
                Id = display.Id,
                Name = display.Name
            };

            return Ok(displayApi);
        } catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = string.Format(Resource.ManagerWeb_DisplayIdRetreivalError, id) });
        }
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<DisplayApi>>> GetAllDisplays()
    {
        try
        {
            var displays = await _displayService.GetAllAsync();
        
            var displaysApi = new List<DisplayApi>();
        
            foreach (var display in displays)
            {
                var displayApi = new DisplayApi
                {
                    Id = display.Id,
                    Name = display.Name,
                    ImgUrl = display.ImgUrl
                };
            
                displaysApi.Add(displayApi);
            }
        
            return Ok(displaysApi);
        }
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = Resource.ManagerWeb_DisplaysRetreivalError });
        }
    }
}