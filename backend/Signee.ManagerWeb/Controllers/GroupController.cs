using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signee.Domain.Entities.Display;
using Signee.Domain.Entities.Group;
using Signee.Domain.Entities.View;
using Signee.ManagerWeb.Models.Display;
using Signee.ManagerWeb.Models.Group;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Group.Contracts;

namespace Signee.ManagerWeb.Controllers;


[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class GroupController : ControllerBase
{

    private readonly IGroupService _groupService;
    private readonly IDisplayService _displayService;


    public GroupController(IGroupService groupService, IDisplayService displayService)
    {
        _groupService = groupService;
        _displayService = displayService;
    }
    
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetAllGroups()
    {
        try
        {
            var groups = await _groupService.GetAll();
        
            var groupDtos = groups.Select(group => new GroupDto
            {
                Id = group.Id!,
                Name = group.Name!,
                Displays = group.Displays.Select(d => new DisplayApi()
                {
                    Id = d.Id,
                    Name = d.Name,
                    PairingCode = d.PairingCode,
                    GroupId = d.GroupId
                }).ToList(),      
                Views = group.Views!.Select(v => v.Id).ToList()
            }).ToList();
            
            return Ok(groupDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroupById(string id)
    {
        try
        {
            var group = await _groupService.GetById(id);
            if (group == null)
                return NotFound();

            return Ok(group);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpPost("")]
    public async Task<ActionResult<GroupDto>> CreateGroup([FromBody] CreateGroupApi requestCreateGroup)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Extract UserID from the claim we provided user to create JWT Token
            ClaimsPrincipal currentUser = this.User;
            var authorizedUser = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            
            var group = new Group
            {
                Name = requestCreateGroup.Name,
                Displays = new List<Display>(),
                Views = new List<View>(),
                UserId = authorizedUser
            };

            // Add the group to the database
            await _groupService.Add(group, authorizedUser);
            
            // Construct a DTO for the created group
            var groupDto = new GroupDto
            {
                Id = group.Id!,
                Name = requestCreateGroup.Name!,
                Displays = new List<DisplayApi>(),
                Views = new List<string>()     
            };
            
            return Ok(groupDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

   
    [Authorize(Roles = "Admin, User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(string id, [FromBody] UpdateGroupApi requestUpdateGroup)
    {
        try
        {
            var group = await _groupService.GetById(id);
            if (group == null)
                return NotFound();

            group.Name = requestUpdateGroup.Name;
            await _groupService.Update(group);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(string id)
    {
        try
        {
            var group = await _groupService.GetById(id);
            if (group == null)
                return NotFound();

            await _groupService.DeleteById(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpPost("{groupId}/display/{displayId}")]
    public async Task<IActionResult> AddDisplayToGroup(string groupId, string displayId)
    {
        try
        {
            await _groupService.AddDisplayToGroup(groupId, displayId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("{groupId}/display/{displayId}")]
    public async Task<IActionResult> RemoveDisplayFromGroup(string groupId, string displayId)
    {
        try
        {
            await _groupService.DeleteDisplayFromGroup(groupId, displayId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
}
