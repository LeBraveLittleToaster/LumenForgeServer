using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Auth.Dto.Command;

/// <summary>
/// Payload for creating a user record from a Keycloak subject identifier.
/// </summary>
public record AddUserDto
{
    /// <summary>
    /// Keycloak subject identifier ("sub") for the user to create.
    /// </summary>
    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    [JsonPropertyName("userKcId")]
    public required string userKcId { get; init; }
}
