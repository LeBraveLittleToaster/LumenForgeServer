namespace LumenForgeServer.Auth.Dto;

/// <summary>
/// Payload for creating a user record from a Keycloak subject identifier.
/// </summary>
public record AddUserDto
{
    /// <summary>
    /// Keycloak subject identifier ("sub") for the user to create.
    /// </summary>
    public required string keycloakId { get; init; }
}
