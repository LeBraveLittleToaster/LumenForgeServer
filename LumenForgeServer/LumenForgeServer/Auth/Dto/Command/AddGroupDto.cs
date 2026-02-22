namespace LumenForgeServer.Auth.Dto;

/// <summary>
/// Payload for creating a group with a name and description.
/// </summary>
public record AddGroupDto
{
    /// <summary>
    /// Human-readable name for the group.
    /// </summary>
    public required string Name { get; set; } = null!;
    /// <summary>
    /// Short description explaining the group's purpose.
    /// </summary>
    public required string Description { get; set; }
}
