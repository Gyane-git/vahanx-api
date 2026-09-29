using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Infrastructure.Services;

/// <summary>
/// Health check for SQL Server database connectivity.
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly VahanXDbContext _context;

    public DatabaseHealthCheck(VahanXDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("SQL Server database connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("SQL Server database connection failed.", ex);
        }
    }
}
