namespace LumenForgeServer.Auth.Dto;

/// <summary>
/// Payload for assigning a user to a group.
/// </summary>
public record AssignUserToGroupDto
{
    /// <summary>
    /// Optional Keycloak subject identifier for the actor performing the assignment.
    /// </summary>
    public string? assigneeKeycloakId;
    /// <summary>
    /// Keycloak subject identifier for the user being assigned.
    /// </summary>
    public required string keycloakId;
    /// <summary>
    /// Group identifier for the target group.
    /// </summary>
    public Guid groupGuid;
}
