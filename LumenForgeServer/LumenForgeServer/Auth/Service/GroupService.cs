using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Factory;
using LumenForgeServer.Auth.Persistance;
using LumenForgeServer.Auth.Validator;
using LumenForgeServer.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Auth.Service;

/// <summary>
/// Application service for group-related auth operations.
/// </summary>
public class GroupService(IAuthRepository authRepository) : ControllerBase
{

    /// <summary>
    /// Resolves a group id by group guid.
    /// </summary>
    /// <param name="guid">Group guid to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The internal group id.</returns>
    /// <exception cref="NotFoundException">Thrown when the group cannot be found.</exception>
    public async Task<long> GetGroupByGuid(Guid guid, CancellationToken ct)
    {
        var groupId = await authRepository.GetGroupIdByGuidAsync(guid, ct);
        return groupId;
    }

    /// <summary>
    /// Creates a user record from a payload.
    /// </summary>
    /// <param name="addUserDto">Payload containing the Keycloak subject identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created user.</returns>
    /// <exception cref="ValidationException">Thrown when the payload fails validation.</exception>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">Thrown when persistence fails.</exception>
    public async Task<User?> AddUser(AddUserDto addUserDto, CancellationToken ct)
    {
        var user = UserFactory.BuildUser(addUserDto);
        
        UserValidator.ValidateAddUser(addUserDto);
        
        await authRepository.AddUserAsync(user, ct);
        await authRepository.SaveChangesAsync(ct);
        
        return user;
    }

    /// <summary>
    /// Assigns a user to a group.
    /// </summary>
    /// <param name="dto">Payload describing the assignment.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="ValidationException">Thrown when the payload fails validation.</exception>
    /// <exception cref="NotFoundException">Thrown when the user or group cannot be found.</exception>
    public async Task AssignUserToGroup(AssignUserToGroupDto dto, CancellationToken ct)
    {
        UserValidator.ValidateAssignUserToGroup(dto);
        
        await authRepository.AssignUserToGroupAsync(dto.assigneeKeycloakId, dto.keycloakId, dto.groupGuid, ct);
        await authRepository.SaveChangesAsync(ct);
    }
    
    /// <summary>
    /// Retrieves all roles assigned to a user via group memberships.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Distinct roles assigned to the user.</returns>
    public async Task<HashSet<Role>> GetRolesForKeycloakId(string keycloakId, CancellationToken ct)
    {
        return await authRepository.GetRolesForKeycloakIdAsync(keycloakId,ct);
    }
}
