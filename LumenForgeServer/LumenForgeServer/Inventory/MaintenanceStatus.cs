using System;
using System.Collections.Generic;

namespace RentalDomain.Inventory;

public class MaintenanceStatus
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<Device> Devices { get; set; } = new();
}
