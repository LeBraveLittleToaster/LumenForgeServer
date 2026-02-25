using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.Auth.Dto;

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
    public required string userKcId { get; init; }
}
