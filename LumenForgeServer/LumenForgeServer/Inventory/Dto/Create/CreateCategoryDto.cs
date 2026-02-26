using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for creating an inventory category.
/// </summary>
public record CreateCategoryDto
{
    /// <summary>
    /// Category name.
    /// </summary>
    [Required]
    [StringLength(256, MinimumLength = 1)]
    [RegularExpression(@".*\S.*")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Optional category description.
    /// </summary>
    [StringLength(2000)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
