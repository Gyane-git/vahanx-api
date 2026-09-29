using System.Diagnostics;

namespace VahanX.Api.Middleware;

/// <summary>
/// Middleware that logs request timing information.
/// </summary>
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        _logger.LogInformation("Request {Method} {Path} started", requestMethod, requestPath);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            if (statusCode >= 400)
            {
                _logger.LogWarning(
                    "Request {Method} {Path} completed with status {StatusCode} in {ElapsedMs}ms",
                    requestMethod, requestPath, statusCode, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "Request {Method} {Path} completed with status {StatusCode} in {ElapsedMs}ms",
                    requestMethod, requestPath, statusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
