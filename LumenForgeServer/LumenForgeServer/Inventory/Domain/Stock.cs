using LumenForgeServer.Common;
using LumenForgeServer.Maintenance.Domain;
using LumenForgeServer.Rentals.Domain;

namespace LumenForgeServer.Inventory.Domain;

public class Stock
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }

    public long DeviceId { get; set; }
    public Device Device { get; set; } = null!;

    public StockUnitType UnitStockType { get; set; }
    public decimal StockCount { get; set; } // >= 0

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<RentalItem> RentalItems { get; set; } = new();
    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
