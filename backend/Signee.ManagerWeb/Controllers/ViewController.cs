using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Entities.View;
using Signee.ManagerWeb.Models.View;
using Signee.Services.Areas.View.Contracts;

namespace Signee.ManagerWeb.Controllers;


[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class ViewController : ControllerBase
{
    private readonly IViewService _viewService;
    
    public ViewController(IViewService viewService)
    {
        _viewService = viewService;
    }
    
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<View>>> GetAllViews()
    {
        try
        {
            var views = await _viewService.GetAllAsync();
            return Ok(views);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost("")]
    public async Task<ActionResult<IEnumerable<View>>> CreateView([FromBody] CreateViewApi requestCreateView)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var view = new View
            {
                From = requestCreateView.From,
                To = requestCreateView.To
            };

            await _viewService.AddAsync(view);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}