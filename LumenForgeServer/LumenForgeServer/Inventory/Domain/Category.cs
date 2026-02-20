using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

/// <summary>
/// Represents a category used to classify devices.
/// </summary>
public class Category
{
    /// <summary>
    /// Internal database identifier.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// External identifier used for API interactions.
    /// </summary>
    public Guid Guid { get; set; }
    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Optional category description.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Timestamp when the category was created.
    /// </summary>
    public Instant CreatedAt { get; set; }
    /// <summary>
    /// Timestamp when the category was last updated.
    /// </summary>
    public Instant UpdatedAt { get; set; }

    /// <summary>
    /// Device links for this category.
    /// </summary>
    public List<DeviceCategory> DeviceCategories { get; set; } = new();
}
