namespace LumenForgeServer.Common.Exceptions;

/// <summary>
/// Represents a validation failure with field-level errors.
/// </summary>
/// <param name="message">Human-readable description of the validation failure.</param>
/// <param name="errors">Field-level errors keyed by field name.</param>
/// <param name="innerException">Optional inner exception.</param>
public sealed class ValidationException(
    string message,
    IDictionary<string, string[]> errors,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    /// <summary>
    /// Field-level validation errors keyed by field name.
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(errors);
}
