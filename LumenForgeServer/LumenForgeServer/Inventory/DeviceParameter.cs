using System;

namespace RentalDomain.Inventory;

public class DeviceParameter
{
    public long Id { get; set; }
    public string ParamKey { get; set; } = null!;
    public string Value { get; set; } = null!;

    public long DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
