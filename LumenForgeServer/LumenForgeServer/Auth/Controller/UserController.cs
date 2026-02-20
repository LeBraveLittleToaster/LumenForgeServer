using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Controller;

[Route("api/v1/auth/user")]
[ApiController]
[Authorize] 
public class UserController(UserService userService) : ControllerBase
{
    [HttpPost("add")]
    [Authorize(Roles = "REALM_ADMIN")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    
    public async Task<IActionResult> AddUser([FromBody] AddUserDto addUserDto, CancellationToken ct)
    {
        var user = await userService.AddUser(addUserDto, ct);
        
        return CreatedAtAction(nameof(GetUser), new { keycloakId = user?.KeycloakUserId }, user);
    }

    [HttpGet("{keycloakId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    public async Task<IActionResult> GetUser(string keycloakId, CancellationToken ct)
    {
        var user = await userService.GetUserByKeycloakId(keycloakId, ct);
        return new JsonResult(user);
    }

    [HttpGet("{keycloakId}/roles")]
    public async Task<IActionResult> GetUserRoles(string keycloakId, CancellationToken ct)
    {
        var roles = await userService.GetRolesForKeycloakId(keycloakId, ct);
        return Ok(roles);
    }
}