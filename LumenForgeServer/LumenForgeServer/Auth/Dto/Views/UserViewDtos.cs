using System.Text.Json.Serialization;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Common.Database;
using NodaTime;

namespace LumenForgeServer.Auth.Dto.Views;

/// <summary>
/// Subset of the User class for API response
/// </summary>
public record UserView
{
    /// <summary>
    /// Timestamp when the user joined the system.
    /// </summary>
    [JsonPropertyName("joined_at")]
    public required Instant JoinedAt { get; set; } 
    /// <summary>
    /// Keycloak subject identifier ("sub") for the user.
    /// </summary>
    [JsonPropertyName("user_kc_id")]
    public required string UserKcId { get; set; }
    /// <summary>
    /// Group memberships for the user.
    /// </summary>
    [JsonPropertyName("group_users")]
    public List<GroupUser> GroupUsers { get; private set; } = [];

    public static UserView FromEntity(KcUserReference tEntity)
    {
        return new UserView
        {
            UserKcId = tEntity.UserKcId,
            JoinedAt = tEntity.JoinedAt,
            GroupUsers = tEntity.GroupUsers
        };
    }
}