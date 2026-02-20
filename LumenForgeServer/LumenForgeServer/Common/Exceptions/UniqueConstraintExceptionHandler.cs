using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Common.Exceptions
{
    /// <summary>
    /// Handles <see cref="UniqueConstraintException"/> by returning a 409 ProblemDetails response.
    /// </summary>
    /// <remarks>
    /// Maps unique constraint violations to HTTP 409 responses.
    /// </remarks>
    internal sealed class UniqueConstraintExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<UniqueConstraintExceptionHandler> logger) : IExceptionHandler
    {
        /// <summary>
        /// Attempts to handle a unique constraint exception.
        /// </summary>
        /// <param name="httpContext">Current HTTP context.</param>
        /// <param name="exception">Exception to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><c>true</c> when the response was written; otherwise <c>false</c>.</returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not UniqueConstraintException)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            httpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Validation failed",
                Detail = exception.Message,
                Status = StatusCodes.Status409Conflict,
            };

            var written = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });

            logger.LogInformation("After write: written={Written} Status={Status} Started={Started}",
                written,
                httpContext.Response.StatusCode,
                httpContext.Response.HasStarted);

            return written;
        }
    }
}
