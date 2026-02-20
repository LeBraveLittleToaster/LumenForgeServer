namespace LumenForgeServer.Common.Exceptions;

/// <summary>
/// Indicates a database unique constraint violation.
/// </summary>
/// <param name="message">Description of the constraint violation (not passed to the base exception).</param>
/// <param name="e">Inner exception from the data layer (not passed to the base exception).</param>
/// <remarks>
/// This exception currently does not forward its constructor parameters to <see cref="Exception"/>.
/// </remarks>
public class UniqueConstraintException(string message, Exception e) : Exception { }
