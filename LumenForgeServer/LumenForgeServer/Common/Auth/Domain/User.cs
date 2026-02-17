namespace LumenForgeServer.Common.Auth.Domain;

public class User
{
    public long Id { get; set; }
    public required string KeycloakUserId { get; set; }
    public List<GroupUser> GroupUsers { get; } = [];
}