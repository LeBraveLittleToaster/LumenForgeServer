namespace LumenForgeServer.Common.Auth.Domain;

public class GroupRole
{
    public long GroupId { get; set; }
    public Role RoleId { get; set; }

    public Group Group { get; set; } = null!;
}