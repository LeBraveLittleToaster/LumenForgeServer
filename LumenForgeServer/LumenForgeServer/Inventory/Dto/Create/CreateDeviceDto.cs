namespace LumenForgeServer.Inventory.Dto.Create;

public record CreateDeviceDto
{
    public string SerialNumber { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid vendorUuid { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public CreateStockDto Stock { get; set; }
    public List<CreateDeviceParameterDto> Parameters { get; set; }
    public List<Guid> CategoriesUuids { get; set; }
}



