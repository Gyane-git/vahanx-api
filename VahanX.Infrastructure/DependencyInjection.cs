using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VahanX.Application.Common.Interfaces;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using VahanX.Infrastructure.Services;

namespace VahanX.Infrastructure;

/// <summary>
/// Extension methods for registering Infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VahanXDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(VahanXDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(3);
                });
        });

        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // Generic Repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddHealthChecks()
            .AddDbContextCheck<VahanXDbContext>(
                name: "database",
                tags: ["db", "sql"],
                customTestQuery: null);

        return services;
    }
}
