using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Entities.Group;
using Signee.ManagerWeb.Models.Group;
using Signee.Services.Areas.Group.Contracts;

namespace Signee.ManagerWeb.Controllers;


[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class GroupController : ControllerBase
{

    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Group>>> GetAllGroups()
    {
        try
        {
            var groups = await _groupService.GetAllAsync();
            return Ok(groups);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    [HttpPost("")]
    public async Task<ActionResult<IEnumerable<Group>>> CreateGroup([FromBody] CreateGroupApi requestCreateGroup)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var group = new Group
            {
                Name = requestCreateGroup.Name
            };

            await _groupService.AddAsync(group);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
}
