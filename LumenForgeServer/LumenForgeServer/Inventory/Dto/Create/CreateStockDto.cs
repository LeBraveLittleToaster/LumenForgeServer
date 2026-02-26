using LumenForgeServer.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Inventory.Dto.Create;

/// <summary>
/// Payload for creating stock information for a device.
/// </summary>
public record CreateStockDto
{
    /// <summary>
    /// Unit type used to interpret the stock count.
    /// </summary>
    [JsonPropertyName("stockUnitType")]
    public StockUnitType StockUnitType { get; set; }
    /// <summary>
    /// Quantity available in stock.
    /// </summary>
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    [JsonPropertyName("stockCount")]
    public decimal StockCount { get; set; }
}
