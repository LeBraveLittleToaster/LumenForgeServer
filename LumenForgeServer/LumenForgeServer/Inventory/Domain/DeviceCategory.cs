namespace LumenForgeServer.Inventory.Domain;

public class DeviceCategory
{
    public long Id { get; set; }

    public long DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
