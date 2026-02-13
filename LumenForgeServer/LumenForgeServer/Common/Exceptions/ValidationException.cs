namespace LumenForgeServer.Common.Exceptions;

public sealed class ValidationException(
    string message,
    IDictionary<string, string[]> errors,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(errors);
}