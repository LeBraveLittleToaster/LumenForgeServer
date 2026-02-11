using System;
using System.Collections.Generic;

namespace RentalDomain.Inventory;

public class Device
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string? DeviceName { get; set; }
    public string? DeviceDescription { get; set; }
    public string? PhotoUrl { get; set; }

    public long VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;

    public decimal PurchasePrice { get; set; }
    public DateOnly PurchaseDate { get; set; }

    public long MaintenanceStatusId { get; set; }
    public MaintenanceStatus MaintenanceStatus { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Stock Stock { get; set; } = null!;
    public List<DeviceParameter> Parameters { get; set; } = new();
    public List<DeviceCategory> DeviceCategories { get; set; } = new();
}
