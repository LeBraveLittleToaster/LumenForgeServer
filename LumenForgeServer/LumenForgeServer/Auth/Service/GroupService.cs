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
    public async Task<User?> AddUser(string userKcId, CancellationToken ct)
    {
        var user = UserFactory.BuildUser(userKcId);
        
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
    public async Task AssignUserToGroup(string? assigneeKcId, string userKcId, Guid groupGuid, CancellationToken ct)
    {
        await authRepository.AssignUserToGroupAsync(assigneeKcId, userKcId, groupGuid, ct);
        await authRepository.SaveChangesAsync(ct);
    }
}
