using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

public class Category
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<DeviceCategory> DeviceCategories { get; set; } = new();
}
