using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Inventory.Dto.Update;

/// <summary>
/// Payload for replacing all category assignments on a device.
/// </summary>
public sealed record SetDeviceCategoriesDto
{
    /// <summary>
    /// Full list of category GUIDs to assign to the device.
    /// </summary>
    [Required]
    [JsonPropertyName("categoryGuids")]
    public List<Guid> CategoryGuids { get; init; } = [];
}
