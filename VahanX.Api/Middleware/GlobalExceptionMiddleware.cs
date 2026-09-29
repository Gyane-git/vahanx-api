using System.Net;
using System.Text.Json;
using VahanX.Application.Common;
using VahanX.Domain.Exceptions;

namespace VahanX.Api.Middleware;

/// <summary>
/// Global exception middleware that catches all unhandled exceptions
/// and returns consistent API responses.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = Guid.NewGuid().ToString();
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            ValidationException validationEx =>
                ((int)HttpStatusCode.BadRequest, "Validation failed", validationEx.Errors
                    .SelectMany(kvp => kvp.Value.Select(v => new ApiError
                    {
                        Code = "VALIDATION_ERROR",
                        Field = kvp.Key,
                        Message = v
                    }))
                    .ToList()),

            NotFoundException notFoundEx =>
                ((int)HttpStatusCode.NotFound, notFoundEx.Message, new List<ApiError>
                {
                    new() { Code = "NOT_FOUND", Field = string.Empty, Message = notFoundEx.Message }
                }),

            ConflictException conflictEx =>
                ((int)HttpStatusCode.Conflict, conflictEx.Message, new List<ApiError>
                {
                    new() { Code = "CONFLICT", Field = string.Empty, Message = conflictEx.Message }
                }),

            UnauthorizedException unauthorizedEx =>
                ((int)HttpStatusCode.Unauthorized, unauthorizedEx.Message, new List<ApiError>
                {
                    new() { Code = "UNAUTHORIZED", Field = string.Empty, Message = unauthorizedEx.Message }
                }),

            ForbiddenException forbiddenEx =>
                ((int)HttpStatusCode.Forbidden, forbiddenEx.Message, new List<ApiError>
                {
                    new() { Code = "FORBIDDEN", Field = string.Empty, Message = forbiddenEx.Message }
                }),

            DomainException domainEx =>
                ((int)HttpStatusCode.BadRequest, domainEx.Message, new List<ApiError>
                {
                    new() { Code = domainEx.ErrorCode, Field = string.Empty, Message = domainEx.Message }
                }),

            _ =>
                ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.", new List<ApiError>
                {
                    new() { Code = "INTERNAL_ERROR", Field = string.Empty, Message = "An unexpected error occurred." }
                })
        };

        context.Response.StatusCode = statusCode;

        if (statusCode == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);
        }
        else
        {
            _logger.LogWarning(exception, "Domain exception occurred. TraceId: {TraceId}", traceId);
        }

        var response = ApiResponse<object>.ErrorResponse(message, errors);
        response.TraceId = traceId;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
