using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for creating an inventory category.
/// </summary>
public record CreateCategoryDTO
{
    /// <summary>
    /// Category name.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Optional category description.
    /// </summary>
    public string? Description { get; set; }
}
