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

        // Marketplace Services
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<ISellerService, SellerService>();
        services.AddScoped<IDealerService, DealerService>();
        services.AddScoped<IListingMediaService, ListingMediaService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<ICompareService, CompareService>();

        // Trust Services
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<IInspectionService, InspectionService>();
        services.AddScoped<IVehicleHistoryService, VehicleHistoryService>();
        services.AddScoped<IReviewService, ReviewService>();

        // Engagement Services
        services.AddScoped<IEnquiryService, EnquiryService>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<ITestDriveService, TestDriveService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
