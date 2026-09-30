using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Admin;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of admin dashboard service.
/// </summary>
public class AdminDashboardService : IAdminDashboardService
{
    private readonly IRepository<User> _users;
    private readonly IRepository<VehicleListing> _listings;
    private readonly IRepository<Seller> _sellers;
    private readonly IRepository<Dealer> _dealers;
    private readonly IRepository<VehicleVerification> _verifications;
    private readonly IRepository<Inspection> _inspections;
    private readonly IRepository<Report> _reports;
    private readonly IRepository<ModerationCase> _moderationCases;
    private readonly IRepository<Advertisement> _advertisements;
    private readonly IRepository<Subscription> _subscriptions;
    private readonly IRepository<ServiceBooking> _serviceBookings;
    private readonly IRepository<SellRequest> _sellRequests;
    private readonly IRepository<Payment> _payments;

    public AdminDashboardService(
        IRepository<User> users,
        IRepository<VehicleListing> listings,
        IRepository<Seller> sellers,
        IRepository<Dealer> dealers,
        IRepository<VehicleVerification> verifications,
        IRepository<Inspection> inspections,
        IRepository<Report> reports,
        IRepository<ModerationCase> moderationCases,
        IRepository<Advertisement> advertisements,
        IRepository<Subscription> subscriptions,
        IRepository<ServiceBooking> serviceBookings,
        IRepository<SellRequest> sellRequests,
        IRepository<Payment> payments)
    {
        _users = users;
        _listings = listings;
        _sellers = sellers;
        _dealers = dealers;
        _verifications = verifications;
        _inspections = inspections;
        _reports = reports;
        _moderationCases = moderationCases;
        _advertisements = advertisements;
        _subscriptions = subscriptions;
        _serviceBookings = serviceBookings;
        _sellRequests = sellRequests;
        _payments = payments;
    }

    public async Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var totalUsers = await _users.CountAsync(null, cancellationToken);
        var activeUsers = await _users.CountAsync(u => u.IsActive, cancellationToken);
        var newUsersToday = await _users.CountAsync(u => u.CreatedAt >= today, cancellationToken);
        var newUsersThisMonth = await _users.CountAsync(u => u.CreatedAt >= startOfMonth, cancellationToken);

        var totalListings = await _listings.CountAsync(null, cancellationToken);
        var publishedListings = await _listings.CountAsync(l => l.Status == ListingStatus.Published, cancellationToken);
        var pendingListings = await _listings.CountAsync(l => l.Status == ListingStatus.PendingReview, cancellationToken);
        var soldListings = await _listings.CountAsync(l => l.Status == ListingStatus.Sold, cancellationToken);

        var totalSellers = await _sellers.CountAsync(null, cancellationToken);
        var totalDealers = await _dealers.CountAsync(null, cancellationToken);

        var pendingVerifications = await _verifications.CountAsync(v => v.Status == VehicleVerificationStatus.Pending || v.Status == VehicleVerificationStatus.InReview, cancellationToken);
        var pendingInspections = await _inspections.CountAsync(i => i.Status == InspectionStatus.Scheduled, cancellationToken);
        var pendingReports = await _reports.CountAsync(r => r.Status == ModerationReportStatus.Pending, cancellationToken);
        var pendingModerationCases = await _moderationCases.CountAsync(mc => mc.Status == ModerationCaseStatus.Open || mc.Status == ModerationCaseStatus.InProgress, cancellationToken);

        var activeAdvertisements = await _advertisements.CountAsync(a => a.Status == AdvertisementStatus.Active, cancellationToken);
        var activeSubscriptions = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);

        var serviceBookingsToday = await _serviceBookings.CountAsync(sb => sb.CreatedAt >= today, cancellationToken);
        var sellRequestsOpen = await _sellRequests.CountAsync(sr => sr.Status == SellRequestStatus.Submitted || sr.Status == SellRequestStatus.UnderReview, cancellationToken);

        var revenueToday = await _payments.CountAsync(p => p.Status == PaymentStatus.Succeeded && p.CompletedAt >= today, cancellationToken);
        var revenueThisMonth = await _payments.CountAsync(p => p.Status == PaymentStatus.Succeeded && p.CompletedAt >= startOfMonth, cancellationToken);

        return new AdminDashboardSummaryDto
        {
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            NewUsersToday = newUsersToday,
            NewUsersThisMonth = newUsersThisMonth,
            TotalListings = totalListings,
            PublishedListings = publishedListings,
            PendingListings = pendingListings,
            SoldListings = soldListings,
            TotalSellers = totalSellers,
            TotalDealers = totalDealers,
            PendingVerifications = pendingVerifications,
            PendingInspections = pendingInspections,
            PendingReports = pendingReports,
            PendingModerationCases = pendingModerationCases,
            ActiveAdvertisements = activeAdvertisements,
            ActiveSubscriptions = activeSubscriptions,
            ServiceBookingsToday = serviceBookingsToday,
            SellRequestsOpen = sellRequestsOpen,
            RevenueToday = revenueToday,
            RevenueThisMonth = revenueThisMonth
        };
    }
}
