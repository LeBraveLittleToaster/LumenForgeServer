using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

/// <summary>
/// Represents a vendor that supplies devices.
/// </summary>
public class Vendor
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
    /// Vendor name.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Timestamp when the vendor was created.
    /// </summary>
    public Instant CreatedAt { get; set; }
    /// <summary>
    /// Timestamp when the vendor was last updated.
    /// </summary>
    public Instant UpdatedAt { get; set; }

    /// <summary>
    /// Devices supplied by this vendor.
    /// </summary>
    public List<Device> Devices { get; set; } = new();
}
