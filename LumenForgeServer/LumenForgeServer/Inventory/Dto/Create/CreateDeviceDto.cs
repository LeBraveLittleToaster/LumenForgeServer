namespace LumenForgeServer.Inventory.Dto.Create;

public record CreateDeviceDto
{
    public required string SerialNumber { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid vendorUuid { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public required CreateStockDto Stock { get; set; }
    public required List<CreateDeviceParameterDto> Parameters { get; set; }
    public required List<Guid> CategoriesUuids { get; set; }
}



