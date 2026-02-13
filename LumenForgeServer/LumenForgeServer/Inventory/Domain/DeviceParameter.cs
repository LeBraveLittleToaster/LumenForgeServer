namespace LumenForgeServer.Inventory.Domain;

public class DeviceParameter
{
    public long Id { get; set; }
    public string ParamKey { get; set; } = null!;
    public string Value { get; set; } = null!;

    public long DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
