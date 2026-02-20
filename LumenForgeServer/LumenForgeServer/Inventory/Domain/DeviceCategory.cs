namespace LumenForgeServer.Inventory.Domain;

/// <summary>
/// Join entity linking a device to a category.
/// </summary>
public class DeviceCategory
{
    /// <summary>
    /// Internal database identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Foreign key for the device.
    /// </summary>
    public long DeviceId { get; set; }
    /// <summary>
    /// Device associated with the link.
    /// </summary>
    public Device Device { get; set; } = null!;

    /// <summary>
    /// Foreign key for the category.
    /// </summary>
    public long CategoryId { get; set; }
    /// <summary>
    /// Category associated with the link.
    /// </summary>
    public Category Category { get; set; } = null!;
}
