using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.Auth.Dto;

/// <summary>
/// Payload for creating a group with a name and description.
/// </summary>
public record AddGroupDto
{
    /// <summary>
    /// Human-readable name for the group.
    /// </summary>
    [Required]
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public required string Name { get; set; } = null!;
    /// <summary>
    /// Short description explaining the group's purpose.
    /// </summary>
    [Required]
    [MinLength(10)]
    [RegularExpression(@".*\S.*")]
    public required string Description { get; set; }
}
