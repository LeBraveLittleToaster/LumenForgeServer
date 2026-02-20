using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

/// <summary>
/// Represents a maintenance status entry for devices.
/// </summary>
public class MaintenanceStatus
{
    /// <summary>
    /// Internal database identifier.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// External identifier used for API interactions.
    /// </summary>
    public Guid Uuid { get; set; }
    /// <summary>
    /// Display name for the status.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Optional description of the status.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Timestamp when the status was created.
    /// </summary>
    public Instant CreatedAt { get; set; }
    /// <summary>
    /// Timestamp when the status was last updated.
    /// </summary>
    public Instant UpdatedAt { get; set; }

    /// <summary>
    /// Devices currently assigned to this status.
    /// </summary>
    public List<Device> Devices { get; set; } = new();
}
