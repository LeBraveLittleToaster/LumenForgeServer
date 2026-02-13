namespace LumenForgeServer.Inventory.Domain;

public class Category
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<DeviceCategory> DeviceCategories { get; set; } = new();
}
