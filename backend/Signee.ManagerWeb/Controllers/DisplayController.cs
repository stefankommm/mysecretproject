using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<DisplayResponseApi>> CreateDisplay([FromBody]CreateDisplayApi request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var display = CreateDisplayApi.ToDomainModel(request);
            await _displayService.AddAsync(display);
            var response = DisplayResponseApi.FromDomainModel(display);
            
            return CreatedAtAction(nameof(GetDisplay), response);
        }
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = Resource.ManagerWeb_CreatingDisplayError });
        }
    }
    
    [HttpGet]
    [Produces("application/json")]
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
    
    [Authorize (Roles = "Admin, User")]
    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<DisplayResponseApi>> GetDisplay(string id)
    {
        try
        {
            var display = await _displayService.GetByIdAsync(id);
            var response = DisplayResponseApi.FromDomainModel(display);
            return Ok(response);
        } 
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = string.Format(Resource.ManagerWeb_DisplayIdRetreivalError, id) });
        }
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult> DeleteDisplay(string id)
    {
        try
        {
            await _displayService.DeleteByIdAsync(id);
            return Ok();
        } 
        catch (Exception ex)
        {
            // TODO log exception
            return BadRequest(new { message = string.Format(Resource.ManagerWeb_DisplayDeletionError, id) });
        }
    }
}