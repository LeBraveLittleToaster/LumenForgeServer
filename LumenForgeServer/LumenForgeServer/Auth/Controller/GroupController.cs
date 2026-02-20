using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Service;
using LumenForgeServer.Auth.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Controller;

/// <summary>
/// HTTP API for managing groups in the auth domain.
/// </summary>
/// <remarks>
/// Routes are under <c>api/v1/auth/group</c> and require authenticated access by REALM_ADMIN
/// </remarks>
[Route("api/v1/auth/groups")]
[ApiController]
[Authorize] 
public class GroupController(GroupService groupService) : ControllerBase
{
    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="groupGuid">GroupGuid as string. Identifies the group the user should be assigned to.</param>
    /// <param name="addGroupDto">Payload containing group name and description.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 201 response with the created group payload.</returns>
    /// <exception cref="NotImplementedException">Thrown because this endpoint is not implemented.</exception>
    [HttpPut("{groupGuid}/users")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    
    public async Task<IActionResult> AssignUserToGroup(string groupGuid, [FromBody] AssignUserToGroupDto addGroupDto, CancellationToken ct)
    {
        GroupRequestValidator.ValidateAssignUserToGroupRequest(groupGuid, addGroupDto, out var parsedGroupGuid);
        
        await groupService.AssignUserToGroup(addGroupDto.assigneeKcId, addGroupDto.userKcId, parsedGroupGuid, ct);
        return NoContent();
    }
}
