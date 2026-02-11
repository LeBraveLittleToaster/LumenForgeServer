using System;
using System.Collections.Generic;
using RentalDomain.Common;
using RentalDomain.Maintenance;
using LumenForgeServer.Rentals.domain;

namespace RentalDomain.Inventory;

public class Stock
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public StockUnitType UnitStockType { get; set; }
    public decimal StockCount { get; set; } // >= 0

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<RentalItem> RentalItems { get; set; } = new();
    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
