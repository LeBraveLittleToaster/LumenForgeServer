using LumenForgeServer.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Inventory.Dto.Update;

/// <summary>
/// Payload for updating stock fields on an existing device.
/// </summary>
public sealed class UpdateStockDto : IValidatableObject
{
    /// <summary>
    /// Updated unit type.
    /// </summary>
    [JsonPropertyName("stockUnitType")]
    public StockUnitType? StockUnitType { get; init; }

    /// <summary>
    /// Updated stock quantity.
    /// </summary>
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    [JsonPropertyName("stockCount")]
    public decimal? StockCount { get; init; }

    /// <summary>
    /// Ensures at least one stock field is provided.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StockUnitType is null && StockCount is null)
        {
            yield return new ValidationResult(
                "At least one of StockUnitType or StockCount must be provided.",
                new[] { nameof(StockUnitType), nameof(StockCount) });
        }
    }
}
