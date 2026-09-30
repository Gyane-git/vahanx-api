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

        // Services & Charging
        services.AddScoped<IServiceCenterService, ServiceCenterService>();
        services.AddScoped<IServiceBookingService, ServiceBookingService>();
        services.AddScoped<IChargingStationService, ChargingStationService>();
        services.AddScoped<IFuelStationService, FuelStationService>();

        // Business & Monetization Services
        services.AddScoped<IFeatureService, FeatureService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IAdvertisementService, AdvertisementService>();
        services.AddScoped<ISellVehicleService, SellVehicleService>();
        services.AddScoped<IPaymentService, PaymentService>();

        // Phase 9: Admin, Moderation, Analytics, Audit, CMS
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<IModerationService, ModerationService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<ICmsService, CmsService>();

        return services;
    }
}
