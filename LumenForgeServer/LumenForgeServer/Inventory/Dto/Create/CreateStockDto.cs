using LumenForgeServer.Common;

namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for creating stock information for a device.
/// </summary>
public record CreateStockDto
{
    /// <summary>
    /// Unit type used to interpret the stock count.
    /// </summary>
    public StockUnitType StockUnitType { get; set; }
    /// <summary>
    /// Quantity available in stock.
    /// </summary>
    public decimal StockCount { get; set; }
}
