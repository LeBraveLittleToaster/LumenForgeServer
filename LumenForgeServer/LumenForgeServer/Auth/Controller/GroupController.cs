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
[Route("api/v1/auth/group")]
[ApiController]
[Authorize] 
public class GroupController(UserService userService) : ControllerBase
{
    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="addGroupDto">Payload containing group name and description.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 201 response with the created group payload.</returns>
    /// <exception cref="NotImplementedException">Thrown because this endpoint is not implemented.</exception>
    [HttpPost("add")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    
    public async Task<IActionResult> AddGroup([FromBody] AddGroupDto addGroupDto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Assigns a user to a group.
    /// </summary>
    /// <param name="dto">Payload describing the assignment.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 204 response when the assignment is successful.</returns>
    /// <exception cref="NotImplementedException">Thrown because this endpoint is not implemented.</exception>
    [HttpPost("assign-group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> AssignToGroup([FromBody] AssignUserToGroupDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves role assignments for the specified group.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with the role set.</returns>
    /// <exception cref="NotImplementedException">Thrown because this endpoint is not implemented.</exception>
    [HttpGet("{keycloakId}/roles")]
    public async Task<IActionResult> GetRoles(string keycloakId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
