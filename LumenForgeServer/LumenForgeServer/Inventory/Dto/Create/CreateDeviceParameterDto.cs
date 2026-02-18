namespace LumenForgeServer.Inventory.Dto.Create;

public record CreateDeviceParameterDto
{
    public required string Key { get; set; }
    public required string Value { get; set; }    
}