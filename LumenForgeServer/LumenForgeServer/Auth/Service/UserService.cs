using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Factory;
using LumenForgeServer.Auth.Persistance;
using LumenForgeServer.Auth.Validator;
using LumenForgeServer.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumenForgeServer.Auth.Service;

/// <summary>
/// Application service for user-related auth operations.
/// </summary>
public class UserService(IAuthRepository authRepository) : ControllerBase
{
    /// <summary>
    /// Retrieves a user by Keycloak subject identifier.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The user if found.</returns>
    /// <exception cref="NotFoundException">Thrown when the user cannot be found.</exception>
    public async Task<User?> GetUserByKeycloakId(string keycloakId, CancellationToken ct)
    {
        var user = await authRepository.TryGetUserByKeycloakIdAsync(keycloakId, ct);
        return user ?? throw new NotFoundException($"User with Keycloak ID {keycloakId} not found.");
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

        try
        {
            await authRepository.AddUserAsync(user, ct);
            await authRepository.SaveChangesAsync(ct);
        }
        catch (DbUpdateException e)
        {
            throw new UniqueConstraintException(e.Message, e);
        }

        return user;
    }

    /// <summary>
    /// Deletes a user record from the database.
    /// </summary>
    /// <param name="userKcId">Stable Keycloak subject identifier</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created user.</returns>
    /// <exception cref="ValidationException">Thrown when the payload fails validation.</exception>
    public async Task DeleteUserByKcId(string userKcId, CancellationToken ct)
    {
        await authRepository.DeleteUserByKcIdAsync(userKcId, ct);
        await authRepository.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Retrieves all roles assigned to a user via group memberships.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Distinct roles assigned to the user.</returns>
    public async Task<HashSet<Role>> GetRolesForKcId(string keycloakId, CancellationToken ct)
    {
        return await authRepository.GetRolesForKcIdAsync(keycloakId, ct);
    }
}