using System.ComponentModel.DataAnnotations;

namespace LumenForgeServer.Auth.Dto;

/// <summary>
/// Payload for updating group metadata.
/// </summary>
public sealed class UpdateGroupDto : IValidatableObject
{
    /// <summary>
    /// Updated group name.
    /// </summary>
    [MinLength(1)]
    [RegularExpression(@".*\S.*")]
    public string? Name { get; init; }

    /// <summary>
    /// Updated group description.
    /// </summary>
    [MinLength(10)]
    [RegularExpression(@".*\S.*")]
    public string? Description { get; init; }

    /// <summary>
    /// Ensures at least one field is provided.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Description))
        {
            yield return new ValidationResult(
                "At least one of Name or Description must be provided.",
                new[] { nameof(Name), nameof(Description) });
        }
    }
}
