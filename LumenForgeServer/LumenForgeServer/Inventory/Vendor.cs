using System;
using System.Collections.Generic;

namespace RentalDomain.Inventory;

public class Vendor
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<Device> Devices { get; set; } = new();
}
