using LumenForgeServer.Common;
using LumenForgeServer.Maintenance.Domain;
using LumenForgeServer.Rentals.Domain;
using NodaTime;

namespace LumenForgeServer.Inventory.Domain;

/// <summary>
/// Represents available stock for a device.
/// </summary>
public class Stock
{
    /// <summary>
    /// Internal database identifier.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// External identifier used for API interactions.
    /// </summary>
    public Guid Uuid { get; set; }

    /// <summary>
    /// Foreign key for the device.
    /// </summary>
    public long DeviceId { get; set; }
    /// <summary>
    /// Device associated with the stock record.
    /// </summary>
    public Device Device { get; set; } = null!;

    /// <summary>
    /// Unit type used to interpret the stock count.
    /// </summary>
    public StockUnitType UnitStockType { get; set; }
    /// <summary>
    /// Quantity available in stock.
    /// </summary>
    public decimal StockCount { get; set; } // >= 0

    /// <summary>
    /// Timestamp when the stock record was created.
    /// </summary>
    public Instant CreatedAt { get; set; }
    /// <summary>
    /// Timestamp when the stock record was last updated.
    /// </summary>
    public Instant UpdatedAt { get; set; }

    /// <summary>
    /// Rental items tied to this stock.
    /// </summary>
    public List<RentalItem> RentalItems { get; set; } = new();
    /// <summary>
    /// Maintenance backlog entries tied to this stock.
    /// </summary>
    public List<MaintenanceBacklog> MaintenanceBacklogs { get; set; } = new();
}
