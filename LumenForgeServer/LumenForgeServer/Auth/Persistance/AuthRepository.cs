using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Common.Database;
using LumenForgeServer.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace LumenForgeServer.Auth.Persistance;

public sealed class AuthRepository(AppDbContext _db) : IAuthRepository
    
{
    
    public async Task AddUserAsync(User user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct).AsTask();
        await _db.SaveChangesAsync(ct);
    }

    public Task DeleteUserByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<User?> TryGetUserByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        return _db.Users
            .Where(u => u.KeycloakUserId == keycloakId)
            .SingleOrDefaultAsync(ct);
    }

    public Task<User?> TryGetUserAndGroupsByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        return _db.Users
            .Where(u => u.KeycloakUserId == keycloakId)
            .SingleOrDefaultAsync(ct);
    }

    public async Task<HashSet<Role>> GetRolesForKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        return await _db.Users
            .Where(u => u.KeycloakUserId == keycloakId)
            .SelectMany(u => u.GroupUsers)
            .SelectMany(gu => gu.Group.GroupRoles)
            .Select(gr => gr.RoleId)
            .Distinct()
            .ToHashSetAsync(ct);
    }

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
    
    public async Task<long> GetUserIdByKeycloakIdAsync(string keycloakId, CancellationToken ct)
    {
        var userId = await _db.Users
            .Where(u => u.KeycloakUserId == keycloakId)
            .Select(u => u.Id)
            .SingleOrDefaultAsync(ct);
        return userId == 0 
            ? throw new NotFoundException($"User with keycloakId {keycloakId} not found") 
            : userId;
    }


    public Task AddGroupAsync(Group group, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroupByGuidAsync(Guid guid, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task AssignRoleToGroupAsync(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRoleFromGroupAsync(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

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
    
    public Task RemoveUserFromGroupAsync(Group group, User user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserInGroupAsync(User user, Group group, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasGroupRoleAsync(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}