using NodaTime;

namespace LumenForgeServer.Auth.Domain;

/// <summary>
/// Represents an authenticated user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Internal database identifier.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Timestamp when the user joined the system.
    /// </summary>
    public required Instant JoinedAt { get; set; } 
    /// <summary>
    /// Keycloak subject identifier ("sub") for the user.
    /// </summary>
    public required string UserKcId { get; set; }
    /// <summary>
    /// Group memberships for the user.
    /// </summary>
    public List<GroupUser> GroupUsers { get; } = [];
}
