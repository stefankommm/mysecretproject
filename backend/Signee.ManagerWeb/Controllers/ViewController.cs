using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Entities.View;
using Signee.ManagerWeb.Models.Group;
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
    public async Task<ActionResult<IEnumerable<ViewResponseApi>>> GetAllViews()
    {
        try
        {
            // var views = await _viewService.GetAllByGroupIdAsync();
            // var response = views.Select(v => ViewResponseApi.FromDomainModel(v));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost("")]
    public async Task<ActionResult<ViewResponseApi>> CreateView([FromBody] CreateViewApi request)
    {
        try
        {
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState);
            //
            // var view = CreateViewApi.ToDomainModel(request);
            // await _viewService.AddAsync(view);
            // var response = ViewResponseApi.FromDomainModel(view);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}