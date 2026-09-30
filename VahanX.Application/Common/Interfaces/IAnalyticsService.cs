using VahanX.Application.DTOs.Analytics;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for analytics operations.
/// </summary>
public interface IAnalyticsService
{
    Task<AnalyticsOverviewDto> GetOverviewAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserAnalyticsDto>> GetUserAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ListingAnalyticsDto>> GetListingAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VehicleAnalyticsDto>> GetVehicleAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SearchAnalyticsDto>> GetSearchAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EnquiryAnalyticsDto>> GetEnquiryAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TestDriveAnalyticsDto>> GetTestDriveAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceAnalyticsDto>> GetServiceAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AdvertisementAnalyticsDto>> GetAdvertisementAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SubscriptionAnalyticsDto>> GetSubscriptionAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SellVehicleAnalyticsDto>> GetSellVehicleAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RevenueAnalyticsDto>> GetRevenueAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PlatformAnalyticsDto>> GetPlatformAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default);
}
