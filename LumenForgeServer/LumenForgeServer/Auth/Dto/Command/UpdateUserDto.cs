using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.Auth.Dto;

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
    public string NewUserKcId { get; init; } = null!;
}
