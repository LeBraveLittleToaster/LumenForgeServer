using LumenForgeServer.Common.Auth.Domain;
using LumenForgeServer.Common.Persistance;

namespace LumenForgeServer.Common.Auth.Persistance;

public sealed class UserRepository(AppDbContext _db) : IUserRepository
    
{
    public Task AddUser(User user, CancellationToken ct)
    {
        return _db.Users.AddAsync(user, ct).AsTask();
    }

    public Task DeleteUserByKeycloakId(string keycloakId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<User?> TryGetUserByKeycloakId(string keycloakId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task AddGroup(Group group, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroupByGuid(Guid guid, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task AssignRoleToGroup(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRoleFromGroup(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task AssignUserToGroup(Group group, User user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUserFromGroup(Group group, User user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserInGroup(User user, Group group, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasGroupRole(Group group, Role role, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}