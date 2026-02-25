using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.Auth.Controller;

/// <summary>
/// HTTP API for managing users in the auth domain.
/// </summary>
/// <remarks>
/// Routes are under <c>api/v1/auth/user</c> and require authenticated access.
/// </remarks>
[Route("api/v1/auth/users")]
[ApiController]
[Authorize] 
public class UserController(UserService userService) : ControllerBase
{
    /// <summary>
    /// Creates a user record for a Keycloak subject identifier.
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
    
    public async Task<IActionResult> AddUser([FromBody] AddUserDto addUserDto, CancellationToken ct)
    {
        var user = await userService.AddUser(addUserDto.userKcId, ct);
        
        return CreatedAtAction(nameof(GetUser), new { userKcId = user?.UserKcId }, user);
    }

    /// <summary>
    /// Retrieves a user by Keycloak subject identifier.
    /// </summary>
    /// <param name="userKcId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with the user payload.</returns>
    /// <exception cref="LumenForgeServer.Common.Exceptions.NotFoundException">
    /// Thrown when the user cannot be found.
    /// </exception>
    [HttpGet("{userKcId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    public async Task<IActionResult> GetUser(
        [FromRoute, Required, MinLength(1), RegularExpression(@".*\S.*")]
        string userKcId,
        CancellationToken ct)
    {
        var userView = await userService.GetUserByKeycloakId(userKcId, ct);
        return new JsonResult(userView);
    }
    
    /// <summary>
    /// Deletes a user by Keycloak subject identifier from the local database. User could be still present in keycloak.
    /// </summary>
    /// <param name="userKcId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with the user payload.</returns>
    /// <exception cref="LumenForgeServer.Common.Exceptions.NotFoundException">
    /// Thrown when the user cannot be found.
    /// </exception>
    [HttpDelete("{userKcId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteUserByKcId(
        [FromRoute, Required, MinLength(1), RegularExpression(@".*\S.*")]
        string userKcId,
        CancellationToken ct)
    {
        await userService.DeleteUserByKcId(userKcId, ct);
        return Ok();
    }

    /// <summary>
    /// Retrieves role assignments for the specified user.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A 200 response with the role set.</returns>
    [HttpGet("{keycloakId}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetUserRoles(
        [FromRoute, Required, MinLength(1), RegularExpression(@".*\S.*")]
        string keycloakId,
        CancellationToken ct)
    {
        var roles = await userService.GetRolesForKcId(keycloakId, ct);
        return Ok(roles);
    }
}
