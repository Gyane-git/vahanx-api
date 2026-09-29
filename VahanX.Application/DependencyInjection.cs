using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.Services;

namespace VahanX.Application;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<ISystemInfoService, SystemInfoService>();

        return services;
    }
}
