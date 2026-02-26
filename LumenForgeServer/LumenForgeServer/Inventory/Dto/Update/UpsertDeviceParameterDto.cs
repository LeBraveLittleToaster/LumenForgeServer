using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Inventory.Dto.Update;

/// <summary>
/// Payload for creating or updating a device parameter.
/// </summary>
public sealed record UpsertDeviceParameterDto
{
    /// <summary>
    /// Parameter key to upsert.
    /// </summary>
    [Required]
    [StringLength(256, MinimumLength = 1)]
    [RegularExpression(@".*\S.*")]
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    /// <summary>
    /// Parameter value to set.
    /// </summary>
    [Required]
    [StringLength(4000, MinimumLength = 1)]
    [RegularExpression(@".*\S.*")]
    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
