using LumenForgeServer.Auth.Domain;

namespace LumenForgeServer.Auth.Persistance;

public interface IUserRepository
{
    Task<long> GetUserIdByKeycloakIdAsync(string keycloakId, CancellationToken ct);
    Task AddUserAsync(User user, CancellationToken ct);
    Task DeleteUserByKeycloakIdAsync(string keycloakId, CancellationToken ct);
    Task<User?> TryGetUserByKeycloakIdAsync(string keycloakId, CancellationToken ct);
    Task<HashSet<Role>> GetRolesForKeycloakIdAsync(string keycloakId, CancellationToken ct);
    
    Task<long> GetGroupIdByGuidAsync(Guid groupGuid, CancellationToken ct);
    Task AddGroupAsync(Group group, CancellationToken ct);
    Task DeleteGroupByGuidAsync(Guid guid, CancellationToken ct);
    
    Task AssignRoleToGroupAsync(Group group, Role role, CancellationToken ct);
    Task RemoveRoleFromGroupAsync(Group group, Role role, CancellationToken ct);
    
    Task AssignUserToGroupAsync(string? assigneeKeycloakId, string keycloakId, Guid groupGuid, CancellationToken ct);
    Task RemoveUserFromGroupAsync(Group group, User user, CancellationToken ct);
    
    Task<bool> IsUserInGroupAsync(User user, Group group, CancellationToken ct);
    Task<bool> HasGroupRoleAsync(Group group, Role role, CancellationToken ct);
}