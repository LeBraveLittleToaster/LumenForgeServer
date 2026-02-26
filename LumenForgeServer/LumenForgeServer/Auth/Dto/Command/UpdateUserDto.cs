using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Auth.Dto.Command;

/// <summary>
/// Payload for updating a local user record.
/// </summary>
public sealed record UpdateUserDto
{
    /// <summary>
    /// New Keycloak subject identifier to set for the user.
    /// </summary>
    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    [JsonPropertyName("newUserKcId")]
    public string NewUserKcId { get; init; } = null!;
}
