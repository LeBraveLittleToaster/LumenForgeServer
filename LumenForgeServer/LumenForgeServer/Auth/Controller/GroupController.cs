using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Controller;

[Route("api/v1/auth/group")]
[ApiController]
[Authorize] 
public class GroupController(UserService userService) : ControllerBase
{
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
    
    [HttpPost("assign-group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> AssignToGroup([FromBody] AssignUserToGroupDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{keycloakId}/roles")]
    public async Task<IActionResult> GetRoles(string keycloakId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}