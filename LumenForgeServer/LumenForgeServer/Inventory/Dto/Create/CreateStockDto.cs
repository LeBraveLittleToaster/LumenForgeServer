using LumenForgeServer.Common;

namespace LumenForgeServer.Inventory.Dto.Create;

public record CreateStockDto
{
    public StockUnitType StockUnitType { get; set; }
    public decimal StockCount { get; set; }
}