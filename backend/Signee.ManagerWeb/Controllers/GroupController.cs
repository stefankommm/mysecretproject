using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<GroupResponseApi>>> GetAllGroups()
    {
        try
        {
            var groups = await _groupService.GetAllAsync();
            var response = groups.Select(g => GroupResponseApi.FromDomainModel(g)).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponseApi>> GetGroupById(string id)
    {
        try
        {
            var group = await _groupService.GetByIdAsync(id);
            var response = GroupResponseApi.FromDomainModel(group);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    [HttpPost("")]
    public async Task<ActionResult<GroupResponseApi>> CreateGroup([FromBody] CreateGroupApi request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var group = CreateGroupApi.ToDomainModel(request);
            
            // Extract UserID from the claim we provided user to create JWT Token
            ClaimsPrincipal currentUser = User;
            var authorizedUser = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            group.UserId = authorizedUser;
            
            await _groupService.AddAsync(group);
            var response = GroupResponseApi.FromDomainModel(group);
            
            return Ok(response);
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
            var group = await _groupService.GetByIdAsync(id);
            group.Name = requestUpdateGroup.Name;
            await _groupService.UpdateAsync(group);
            return Ok();
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
            await _groupService.DeleteByIdAsync(id);
            return Ok();
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
            await _groupService.AddDisplayToGroupAsync(groupId, displayId);
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
            await _groupService.RemoveDisplayFromGroupAsync(groupId, displayId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
}
