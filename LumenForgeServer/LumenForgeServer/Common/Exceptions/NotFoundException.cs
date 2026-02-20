namespace LumenForgeServer.Common.Exceptions;

/// <summary>
/// Indicates that a requested entity was not found.
/// </summary>
/// <param name="message">Human-readable description of the missing resource.</param>
public class NotFoundException(string message) : Exception(message);
