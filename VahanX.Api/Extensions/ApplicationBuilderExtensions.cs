using VahanX.Api.Middleware;

namespace VahanX.Api.Extensions;

/// <summary>
/// Extension methods for configuring the HTTP request pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseVahanXApi(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestTimingMiddleware>();
        app.UseMiddleware<GlobalExceptionMiddleware>();

        return app;
    }
}
