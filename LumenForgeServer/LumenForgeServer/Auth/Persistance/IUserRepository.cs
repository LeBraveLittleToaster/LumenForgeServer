using LumenForgeServer.Auth.Domain;

namespace LumenForgeServer.Auth.Persistance;

public interface IUserRepository
{
    Task AddUser(User user, CancellationToken ct);
    Task DeleteUserByKeycloakId(string keycloakId, CancellationToken ct);
    Task<User?> TryGetUserByKeycloakId(string keycloakId, CancellationToken ct);
    
    Task AddGroup(Group group, CancellationToken ct);
    Task DeleteGroupByGuid(Guid guid, CancellationToken ct);
    
    Task AssignRoleToGroup(Group group, Role role, CancellationToken ct);
    Task RemoveRoleFromGroup(Group group, Role role, CancellationToken ct);
    
    Task AssignUserToGroup(Group group, User user, CancellationToken ct);
    Task RemoveUserFromGroup(Group group, User user, CancellationToken ct);
    
    Task<bool> IsUserInGroup(User user, Group group, CancellationToken ct);
    Task<bool> HasGroupRole(Group group, Role role, CancellationToken ct);
}