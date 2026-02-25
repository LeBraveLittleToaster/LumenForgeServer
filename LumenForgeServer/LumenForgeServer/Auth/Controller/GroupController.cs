using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Service;
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
    /// Retrieves a group by groupGuid.
    /// </summary>
    /// <param name="groupGuid">Stable group Guid</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with the group view payload.</returns>
    /// <exception cref="LumenForgeServer.Common.Exceptions.NotFoundException">
    /// Thrown when the group cannot be found.
    /// </exception>
    [HttpGet("{groupGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    public async Task<IActionResult> GetGroup([FromRoute] Guid groupGuid, CancellationToken ct)
    {
        var groupView = await groupService.GetGroupByGuid(groupGuid, ct);
        return new JsonResult(groupView);
    }

    /// <summary>
    /// Creates a group record.
    /// </summary>
    /// <param name="addUserDto">Payload containing the Keycloak subject identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 201 response with the created user payload.</returns>
    [HttpPut("")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]

    public async Task<IActionResult> AddGroup([FromBody] AddGroupDto dto, CancellationToken ct)
    {
        var group = await groupService.AddGroup(dto, ct);

        return CreatedAtAction(nameof(AddGroup), new { groupGuid = group }, group);
    }

    /// <summary>
    /// Delete a group record, identified by Guid.
    /// </summary>
    /// <param name="groupGuid">Path var containing the group Guid.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 204 response with when deleted succesfully.</returns>
    [HttpDelete("{groupGuid:guid}")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]

    public async Task<IActionResult> DeleteGroup([FromRoute] Guid groupGuid, CancellationToken ct)
    {
        await groupService.DeleteGroupByGuid(groupGuid, ct);

        return NoContent();
    }

    /// <summary>
    /// Assigns a user to a group. The user then inherits the role privileges of that group
    /// </summary>
    /// <param name="groupGuid">Group Guid the user is assigned to</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with when assigned successfully.</returns>
    [HttpPut("{groupGuid:guid}/users")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> AssignUserToGroup([FromBody] AssignUserToGroupDto dto, [FromRoute] Guid groupGuid, CancellationToken ct)
    {
        await groupService.AssignUserToGroup(dto.assigneeKcId, dto.userKcId, groupGuid, ct);
        
        return Ok();
    }
}
