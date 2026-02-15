using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Common.Exceptions
{
    internal sealed class UniqueConstraintExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<UniqueConstraintExceptionHandler> logger) : IExceptionHandler
    {
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