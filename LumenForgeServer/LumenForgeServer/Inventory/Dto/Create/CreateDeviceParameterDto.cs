namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for a single device parameter.
/// </summary>
public record CreateDeviceParameterDto
{
    /// <summary>
    /// Parameter key name.
    /// </summary>
    public required string Key { get; set; }
    /// <summary>
    /// Parameter value.
    /// </summary>
    public required string Value { get; set; }    
}
