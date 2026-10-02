using Serilog;
using VahanX.Api.Extensions;
using VahanX.Application;
using VahanX.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation(builder.Configuration);

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseVahanXApi();
app.UseSwaggerDocumentation(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    // Seed development access-control data (roles, permissions, super admin).
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<VahanX.Infrastructure.Persistence.VahanXDbContext>();
        try
        {
            await VahanX.Infrastructure.Persistence.SeedData.AuthenticationSeedData.SeedAsync(context, builder.Configuration);
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "Authentication seed data could not be applied.");
        }
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false, // Liveness only - no checks
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.Run();

// Make Program class public for WebApplicationFactory
public partial class Program { }
