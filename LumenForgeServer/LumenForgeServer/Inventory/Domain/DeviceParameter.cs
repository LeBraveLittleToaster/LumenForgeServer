using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

/// <summary>
/// Represents a typed parameter associated with a device.
/// </summary>
public class DeviceParameter
{
    /// <summary>
    /// Internal database identifier.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Parameter key name.
    /// </summary>
    public string ParamKey { get; set; } = null!;
    /// <summary>
    /// Parameter value.
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>
    /// Foreign key for the device.
    /// </summary>
    public long DeviceId { get; set; }
    /// <summary>
    /// Device this parameter belongs to.
    /// </summary>
    public Device Device { get; set; } = null!;

    /// <summary>
    /// Timestamp when the parameter was created.
    /// </summary>
    public Instant CreatedAt { get; set; }
    /// <summary>
    /// Timestamp when the parameter was last updated.
    /// </summary>
    public Instant UpdatedAt { get; set; }
}
