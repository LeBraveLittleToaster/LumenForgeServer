using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Controller;

[Route("api/v1/user")]
[ApiController]
// You mentioned handling authorization earlier; 
// adding this ensures only authenticated users hit these endpoints.
[Authorize] 
public class UserController(UserService userService) : ControllerBase
{
    [Authorize(Roles = "REALM_ADMIN")]
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> AddUser([FromBody] AddUserDto addUserDto, CancellationToken ct)
    {
        var user = await userService.AddUser(addUserDto, ct);
        
        return CreatedAtAction(nameof(GetUser), new { keycloakId = user?.KeycloakUserId }, user);
    }

    [HttpGet("{keycloakId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string keycloakId, CancellationToken ct)
    {
        var user = await userService.GetUserByKeycloakId(keycloakId, ct);
        return Ok(user);
    }

    [HttpPost("assign-group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignToGroup([FromBody] AssignUserToGroupDto dto, CancellationToken ct)
    {
        await userService.AssignUserToGroup(dto, ct);
        return NoContent();
    }

    [HttpGet("{keycloakId}/roles")]
    public async Task<IActionResult> GetRoles(string keycloakId, CancellationToken ct)
    {
        var roles = await userService.GetRolesForKeycloakId(keycloakId, ct);
        return Ok(roles);
    }
}