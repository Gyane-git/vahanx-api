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

        // Vehicle Core Services
        services.AddScoped<IVehicleTypeService, VehicleTypeService>();
        services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IModelService, ModelService>();
        services.AddScoped<IGenerationService, GenerationService>();
        services.AddScoped<IVariantService, VariantService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IMasterDataService, MasterDataService>();

        return services;
    }
}
