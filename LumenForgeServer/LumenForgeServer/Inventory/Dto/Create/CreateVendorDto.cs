namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for creating a vendor.
/// </summary>
public record CreateVendorDto
{
    /// <summary>
    /// Vendor name.
    /// </summary>
    public required string Name { get; set; }
}   
