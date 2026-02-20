namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for creating a device along with stock, parameters, and category links.
/// </summary>
public record CreateDeviceDto
{
    /// <summary>
    /// Device serial number, expected to be unique.
    /// </summary>
    public required string SerialNumber { get; set; }
    /// <summary>
    /// Human-readable device name.
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Device description or notes.
    /// </summary>
    public required string Description { get; set; }
    /// <summary>
    /// Vendor UUID to associate with the device.
    /// </summary>
    public Guid vendorUuid { get; set; }
    /// <summary>
    /// Purchase date of the device.
    /// </summary>
    public DateOnly PurchaseDate { get; set; }
    /// <summary>
    /// Initial stock details for the device.
    /// </summary>
    public required CreateStockDto Stock { get; set; }
    /// <summary>
    /// Device parameter key/value pairs.
    /// </summary>
    public required List<CreateDeviceParameterDto> Parameters { get; set; }
    /// <summary>
    /// Category UUIDs to associate with the device.
    /// </summary>
    public required List<Guid> CategoriesUuids { get; set; }
}


