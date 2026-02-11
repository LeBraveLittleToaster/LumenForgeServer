using System;

namespace RentalDomain.Inventory;

public class DeviceCategory
{
    public long Id { get; set; }

    public long DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
