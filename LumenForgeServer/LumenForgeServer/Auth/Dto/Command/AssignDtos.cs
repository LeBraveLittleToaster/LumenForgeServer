namespace LumenForgeServer.Auth.Dto;

/// <summary>
/// Payload for assigning a user to a group.
/// </summary>
public record AssignUserToGroupDto
{
    /// <summary>
    /// Optional Keycloak subject identifier for the actor performing the assignment.
    /// </summary>
    public string? assigneeKcId  { get; init; }
    /// <summary>
    /// Keycloak subject identifier for the user being assigned.
    /// </summary>
    public required string userKcId   { get; init; }
}