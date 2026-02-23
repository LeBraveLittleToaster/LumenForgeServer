using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Common.Database;
using LumenForgeServer.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace LumenForgeServer.Auth.Persistance;

/// <summary>
/// EF Core-backed repository for auth users, groups, and roles.
/// </summary>
public sealed class AuthRepository(AppDbContext _db) : IAuthRepository
    
{
    
    /// <summary>
    /// Adds a new user and immediately saves the change.
    /// </summary>
    /// <param name="user">User entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="DbUpdateException">Thrown when persistence fails.</exception>
    public async Task AddUserAsync(User user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct).AsTask();
    }

    /// <summary>
    /// Deletes a user by Keycloak subject identifier.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public async Task DeleteUserByKcIdAsync(string userKcId, CancellationToken ct)
    {
        var user = await _db.Users
            .SingleOrDefaultAsync(u => u.UserKcId == userKcId, ct);
        if( user == null) throw new NotFoundException($"User with keycloakId {userKcId} not found");
        _db.Users.Remove(user);
    }

    /// <summary>
    /// Attempts to retrieve a user by Keycloak subject identifier.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The user if found; otherwise <c>null</c>.</returns>
    public async Task<User?> TryGetUserByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        return await _db.Users
            .Where(u => u.UserKcId == keycloakId)
            .SingleOrDefaultAsync(ct);
    }

    /// <summary>
    /// Attempts to retrieve a user and their group relationships by Keycloak subject identifier.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The user if found; otherwise <c>null</c>.</returns>
    /// <remarks>
    /// This query does not currently include navigation properties.
    /// </remarks>
    public Task<User?> TryGetUserAndGroupsByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        return _db.Users
            .Where(u => u.UserKcId == keycloakId)
            .SingleOrDefaultAsync(ct);
    }

    /// <summary>
    /// Retrieves all roles assigned to a user via group memberships.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Distinct roles assigned to the user.</returns>
    public async Task<HashSet<Role>> GetRolesForKcIdAsync(string keycloakId, CancellationToken ct)
    {
        return await _db.Users
            .Where(u => u.UserKcId == keycloakId)
            .SelectMany(u => u.GroupUsers)
            .SelectMany(gu => gu.Group.GroupRoles)
            .Select(gr => gr.RoleId)
            .Distinct()
            .ToHashSetAsync(ct);
    }

    /// <summary>
    /// Resolves the internal group id for a group guid.
    /// </summary>
    /// <param name="groupGuid">Group guid to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The internal group id.</returns>
    /// <exception cref="NotFoundException">Thrown when the group cannot be found.</exception>
    public async Task<long> GetGroupIdByGuidAsync(Guid groupGuid, CancellationToken ct)
    {
        var groupId = await _db.Groups
            .Where(g => g.Guid == groupGuid)
            .Select(g => g.Id)
            .SingleOrDefaultAsync(ct);

        return groupId == 0 
            ? throw new NotFoundException($"Group {groupId} not found") 
            : groupId;
    }
    
    /// <summary>
    /// Resolves the group for a group guid.
    /// </summary>
    /// <param name="groupGuid">Group guid to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The group object.</returns>
    /// <exception cref="NotFoundException">Thrown when the group cannot be found.</exception>
    public async Task<Group?> GetGroupByGuidAsync(Guid groupGuid, CancellationToken ct)
    {
        var group = await _db.Groups
            .Where(g => g.Guid == groupGuid)
            .SingleOrDefaultAsync(ct);

        return group 
               ?? throw new NotFoundException($"Group {group} not found");
    }
    
    /// <summary>
    /// Resolves the internal user id for a Keycloak subject identifier.
    /// </summary>
    /// <param name="keycloakId">Keycloak subject identifier to look up.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The internal user id.</returns>
    /// <exception cref="NotFoundException">Thrown when the user cannot be found.</exception>
    public async Task<long> GetUserIdByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        var userId = await _db.Users
            .Where(u => u.UserKcId == keycloakId)
            .Select(u => u.Id)
            .SingleOrDefaultAsync(ct);
        return userId == 0 
            ? throw new NotFoundException($"User with keycloakId {keycloakId} not found") 
            : userId;
    }

    /// <summary>
    /// Adds a new group to the persistence store.
    /// </summary>
    /// <param name="group">Group entity to persist.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public async Task AddGroupAsync(Group group, CancellationToken ct)
    {
        await _db.Groups.AddAsync(group, ct).AsTask();
    }

    /// <summary>
    /// Deletes a group by guid.
    /// </summary>
    /// <param name="guid">Group guid to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task DeleteGroupByGuidAsync(Guid guid, CancellationToken ct)
    {
        var groupsRange = _db.Groups
            .Where(g => g.Guid == guid);
        _db.Groups.RemoveRange(groupsRange);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Assigns a role to a group.
    /// </summary>
    /// <param name="group">Group receiving the role.</param>
    /// <param name="role">Role to assign.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task AssignRoleToGroupAsync(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes a role assignment from a group.
    /// </summary>
    /// <param name="group">Group losing the role.</param>
    /// <param name="role">Role to remove.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task RemoveRoleFromGroupAsync(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assigns a user to a group by Keycloak subject identifier and group guid.
    /// </summary>
    /// <param name="assigneeKeycloakId">Optional Keycloak subject identifier for the actor performing the assignment.</param>
    /// <param name="keycloakId">Keycloak subject identifier for the user being assigned.</param>
    /// <param name="groupGuid">Target group guid.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotFoundException">Thrown when the user or group cannot be found.</exception>
    public async Task AssignUserToGroupAsync(string? assigneeKeycloakId, string keycloakId, Guid groupGuid, CancellationToken ct)
    {
        
        var userId = await GetUserIdByKeycloakIdAsync(keycloakId, ct);
        var groupId = await GetGroupIdByGuidAsync(groupGuid, ct);

        if (userId == 0 || groupId == 0)
        {
            throw new NotFoundException(
                $"Uncatched error retrieving user with keycloakId {keycloakId} and groupId {groupId}");
        }

        await _db.GroupUsers.AddAsync(new GroupUser
        {
            AssignedByKeycloakId = assigneeKeycloakId,
            UserId = userId,
            GroupId = groupId,
            JoinedAt = SystemClock.Instance.GetCurrentInstant()
        }, ct);
    }
    
    /// <summary>
    /// Removes a user from a group.
    /// </summary>
    /// <param name="group">Group to remove the user from.</param>
    /// <param name="user">User to remove.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task RemoveUserFromGroupAsync(Group group, User user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks whether a user is a member of a group.
    /// </summary>
    /// <param name="user">User to check.</param>
    /// <param name="group">Group to check.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns><c>true</c> if the user is in the group; otherwise <c>false</c>.</returns>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task<bool> IsUserInGroupAsync(User user, Group group, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks whether a group has a specific role assigned.
    /// </summary>
    /// <param name="group">Group to check.</param>
    /// <param name="role">Role to check.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns><c>true</c> if the role is assigned; otherwise <c>false</c>.</returns>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task<bool> HasGroupRoleAsync(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Persists pending changes to the underlying data store.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="DbUpdateException">Thrown when persistence fails.</exception>
    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}
