using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

public class Vendor
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public Instant CreatedAt { get; set; }
    public Instant UpdatedAt { get; set; }

    public List<Device> Devices { get; set; } = new();
}
