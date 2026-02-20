namespace LumenForgeServer.Inventory.Dto.View;

/// <summary>
/// Read-only view model for presenting category data.
/// </summary>
public record CategoryViewDto
{
    /// <summary>
    /// Category UUID.
    /// </summary>
    public Guid Guid;
    /// <summary>
    /// Category name.
    /// </summary>
    public required string Name;
    /// <summary>
    /// Optional category description.
    /// </summary>
    public string? Description;
}
