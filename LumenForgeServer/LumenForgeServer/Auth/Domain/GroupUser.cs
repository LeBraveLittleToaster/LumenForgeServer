using NodaTime;

namespace LumenForgeServer.Auth.Domain;

public class GroupUser
{
    public long GroupId { get; set; }
    public long UserId { get; set; }

    public Group Group { get; set; } = null!;
    public User User { get; set; } = null!;
    
    public Instant JoinedAt { get; set; }
    public string? AssignedByKeycloakId { get; set; }
}