using NodaTime;

namespace LumenForgeServer.Auth.Domain;

public class Group
{
    public long Id { get; set; }
    public Guid Guid { get; set; }

    public required string Name { get; set; } = null!;
    public required string Description { get; set; }

    public required Instant CreatedAt { get; set; }
    public required Instant UpdatedAt { get; set; }

    public List<GroupUser> GroupUsers { get; } = [];
    public List<GroupRole> GroupRoles { get; } = [];
}
