using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenForgeServer.Auth.Dto.Query;

/// <summary>
/// Paging and search parameters for list endpoints.
/// </summary>
public sealed record ListQueryDto
{
    /// <summary>
    /// Maximum number of records to return.
    /// </summary>
    [Range(1, 200)]
    [JsonPropertyName("limit")]
    public int Limit { get; init; } = 50;

    /// <summary>
    /// Number of records to skip.
    /// </summary>
    [Range(0, int.MaxValue)]
    [JsonPropertyName("offset")]
    public int Offset { get; init; } = 0;

    /// <summary>
    /// Optional search term.
    /// </summary>
    [StringLength(128)]
    [JsonPropertyName("search")]
    public string? Search { get; init; }
}
