using System.Text.Json.Serialization;
using LumenForgeServer.Common;
using LumenForgeServer.Inventory.Domain;
using NodaTime;

namespace LumenForgeServer.Inventory.Dto.View;

/// <summary>
/// View model for device stock.
/// </summary>
public sealed record StockView
{
    [JsonPropertyName("uuid")]
    public Guid Uuid { get; init; }

    [JsonPropertyName("stock_unit_type")]
    public StockUnitType StockUnitType { get; init; }

    [JsonPropertyName("stock_count")]
    public decimal StockCount { get; init; }

    [JsonPropertyName("created_at")]
    public Instant CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public Instant UpdatedAt { get; init; }

    public static StockView FromEntity(Stock stock)
    {
        return new StockView
        {
            Uuid = stock.Uuid,
            StockUnitType = stock.UnitStockType,
            StockCount = stock.StockCount,
            CreatedAt = stock.CreatedAt,
            UpdatedAt = stock.UpdatedAt
        };
    }
}
