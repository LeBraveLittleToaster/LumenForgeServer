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
    /// Retrieves a group by groupGuid.
    /// </summary>
    /// <param name="groupGuid">Stable group Guid</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with the group view payload.</returns>
    /// <exception cref="LumenForgeServer.Common.Exceptions.NotFoundException">
    /// Thrown when the group cannot be found.
    /// </exception>
    [HttpGet("{groupGuid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    public async Task<IActionResult> GetGroup(string groupGuid, CancellationToken ct)
    {
        GroupRequestValidator.ValidateGetGroup(groupGuid, out var parsedGroupGuid);
        
        var groupView = await groupService.GetGroupByGuid(parsedGroupGuid, ct);
        return new JsonResult(groupView);
    }
    
    /// <summary>
    /// Creates a group record.
    /// </summary>
    /// <param name="addUserDto">Payload containing the Keycloak subject identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 201 response with the created user payload.</returns>
    /// <exception cref="LumenForgeServer.Common.Exceptions.ValidationException">
    /// Thrown when the payload fails validation.
    /// </exception>
    [HttpPut("add")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    
    public async Task<IActionResult> AddGroup([FromBody] AddGroupDto dto, CancellationToken ct)
    {
        GroupRequestValidator.ValidateAddGroup(dto);

        var group = await groupService.AddGroup(dto, ct);
        
        return CreatedAtAction(nameof(AddGroup), new { userKcId = group }, group);
    }
}
