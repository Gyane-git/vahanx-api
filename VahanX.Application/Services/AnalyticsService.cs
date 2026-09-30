using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Analytics;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of analytics service.
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<User> _users;
    private readonly IRepository<VehicleListing> _listings;
    private readonly IRepository<ListingAnalytics> _listingAnalytics;
    private readonly IRepository<SearchAnalytics> _searchAnalytics;
    private readonly IRepository<Enquiry> _enquiries;
    private readonly IRepository<TestDrive> _testDrives;
    private readonly IRepository<ServiceBooking> _serviceBookings;
    private readonly IRepository<Advertisement> _advertisements;
    private readonly IRepository<AdvertisementImpression> _adImpressions;
    private readonly IRepository<AdvertisementClick> _adClicks;
    private readonly IRepository<AdvertisementCampaign> _adCampaigns;
    private readonly IRepository<AdvertisementBudget> _adBudgets;
    private readonly IRepository<Subscription> _subscriptions;
    private readonly IRepository<SellRequest> _sellRequests;
    private readonly IRepository<SellOffer> _sellOffers;
    private readonly IRepository<Inspection> _inspections;
    private readonly IRepository<Payment> _payments;
    private readonly IRepository<PaymentRefund> _refunds;

    public AnalyticsService(
        IRepository<User> users,
        IRepository<VehicleListing> listings,
        IRepository<ListingAnalytics> listingAnalytics,
        IRepository<SearchAnalytics> searchAnalytics,
        IRepository<Enquiry> enquiries,
        IRepository<TestDrive> testDrives,
        IRepository<ServiceBooking> serviceBookings,
        IRepository<Advertisement> advertisements,
        IRepository<AdvertisementImpression> adImpressions,
        IRepository<AdvertisementClick> adClicks,
        IRepository<AdvertisementCampaign> adCampaigns,
        IRepository<AdvertisementBudget> adBudgets,
        IRepository<Subscription> subscriptions,
        IRepository<SellRequest> sellRequests,
        IRepository<SellOffer> sellOffers,
        IRepository<Inspection> inspections,
        IRepository<Payment> payments,
        IRepository<PaymentRefund> refunds)
    {
        _users = users;
        _listings = listings;
        _listingAnalytics = listingAnalytics;
        _searchAnalytics = searchAnalytics;
        _enquiries = enquiries;
        _testDrives = testDrives;
        _serviceBookings = serviceBookings;
        _advertisements = advertisements;
        _adImpressions = adImpressions;
        _adClicks = adClicks;
        _adCampaigns = adCampaigns;
        _adBudgets = adBudgets;
        _subscriptions = subscriptions;
        _sellRequests = sellRequests;
        _sellOffers = sellOffers;
        _inspections = inspections;
        _payments = payments;
        _refunds = refunds;
    }

    public async Task<AnalyticsOverviewDto> GetOverviewAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);

        var newUsers = await _users.CountAsync(u => u.CreatedAt >= from && u.CreatedAt <= to, cancellationToken);
        var activeUsers = await _users.CountAsync(u => u.IsActive, cancellationToken);
        var newListings = await _listings.CountAsync(l => l.CreatedAt >= from && l.CreatedAt <= to, cancellationToken);
        var publishedListings = await _listings.CountAsync(l => l.Status == ListingStatus.Published && l.PublishedAt >= from && l.PublishedAt <= to, cancellationToken);
        var enquiries = await _enquiries.CountAsync(e => e.CreatedAt >= from && e.CreatedAt <= to, cancellationToken);
        var testDriveRequests = await _testDrives.CountAsync(td => td.CreatedAt >= from && td.CreatedAt <= to, cancellationToken);
        var serviceBookings = await _serviceBookings.CountAsync(sb => sb.CreatedAt >= from && sb.CreatedAt <= to, cancellationToken);
        var activeAdvertisements = await _advertisements.CountAsync(a => a.Status == AdvertisementStatus.Active, cancellationToken);
        var activeSubscriptions = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
        var sellRequests = await _sellRequests.CountAsync(sr => sr.CreatedAt >= from && sr.CreatedAt <= to, cancellationToken);

        var grossRevenue = await _payments.CountAsync(p => p.Status == PaymentStatus.Succeeded && p.CompletedAt >= from && p.CompletedAt <= to, cancellationToken);
        var refunds = await _refunds.CountAsync(pr => pr.Status == RefundStatus.Succeeded && pr.CompletedAt >= from && pr.CompletedAt <= to, cancellationToken);

        return new AnalyticsOverviewDto
        {
            From = from,
            To = to,
            NewUsers = newUsers,
            ActiveUsers = activeUsers,
            NewListings = newListings,
            PublishedListings = publishedListings,
            Enquiries = enquiries,
            TestDriveRequests = testDriveRequests,
            ServiceBookings = serviceBookings,
            ActiveAdvertisements = activeAdvertisements,
            ActiveSubscriptions = activeSubscriptions,
            SellRequests = sellRequests,
            GrossRevenue = grossRevenue,
            Refunds = refunds,
            NetRevenue = grossRevenue - refunds
        };
    }

    public async Task<IReadOnlyList<UserAnalyticsDto>> GetUserAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<UserAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var newUsers = await _users.CountAsync(u => u.CreatedAt >= date && u.CreatedAt < nextDate, cancellationToken);
            var activeUsers = await _users.CountAsync(u => u.IsActive && u.CreatedAt < nextDate, cancellationToken);
            var totalUsers = await _users.CountAsync(u => u.CreatedAt < nextDate, cancellationToken);

            result.Add(new UserAnalyticsDto
            {
                Date = date,
                NewUsers = newUsers,
                ActiveUsers = activeUsers,
                TotalUsers = totalUsers
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<ListingAnalyticsDto>> GetListingAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<ListingAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var newListings = await _listings.CountAsync(l => l.CreatedAt >= date && l.CreatedAt < nextDate, cancellationToken);
            var publishedListings = await _listings.CountAsync(l => l.Status == ListingStatus.Published && l.PublishedAt >= date && l.PublishedAt < nextDate, cancellationToken);
            var soldListings = await _listings.CountAsync(l => l.Status == ListingStatus.Sold && l.UpdatedAt >= date && l.UpdatedAt < nextDate, cancellationToken);

            var totalViews = await _listingAnalytics.CountAsync(la => la.LastViewedAt >= date && la.LastViewedAt < nextDate, cancellationToken);
            var uniqueViews = await _listingAnalytics.CountAsync(la => la.LastViewedAt >= date && la.LastViewedAt < nextDate, cancellationToken);
            var wishlistAdds = await _listingAnalytics.CountAsync(la => la.LastViewedAt >= date && la.LastViewedAt < nextDate, cancellationToken);
            var enquiries = await _enquiries.CountAsync(e => e.CreatedAt >= date && e.CreatedAt < nextDate, cancellationToken);

            result.Add(new ListingAnalyticsDto
            {
                Date = date,
                NewListings = newListings,
                PublishedListings = publishedListings,
                SoldListings = soldListings,
                TotalViews = totalViews,
                UniqueViews = uniqueViews,
                WishlistAdds = wishlistAdds,
                Enquiries = enquiries
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<VehicleAnalyticsDto>> GetVehicleAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);

        var query = await _listingAnalytics.QueryAsync(true, cancellationToken);
        query = query.Where(la => la.LastViewedAt >= from && la.LastViewedAt <= to);

        var result = await query
            .GroupBy(la => la.ListingId)
            .Select(g => new VehicleAnalyticsDto
            {
                Dimension = "Listing",
                Value = g.Key.ToString(),
                VehicleViews = g.Sum(x => (int)x.Views),
                ListingCount = 1,
                WishlistCount = g.Sum(x => (int)x.WishlistAdds),
                EnquiryCount = g.Sum(x => (int)x.Enquiries),
                TestDriveCount = g.Sum(x => (int)x.TestDriveRequests)
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyList<SearchAnalyticsDto>> GetSearchAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);

        var query = await _searchAnalytics.QueryAsync(true, cancellationToken);
        query = query.Where(sa => sa.CreatedAt >= from && sa.CreatedAt <= to);

        var result = await query
            .GroupBy(sa => sa.SearchQuery)
            .Select(g => new SearchAnalyticsDto
            {
                SearchQuery = g.Key,
                SearchCount = g.Count(),
                AverageResultCount = (int)g.Average(x => x.ResultCount),
                ZeroResultCount = g.Count(x => x.ResultCount == 0)
            })
            .OrderByDescending(sa => sa.SearchCount)
            .Take(50)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyList<EnquiryAnalyticsDto>> GetEnquiryAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<EnquiryAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var created = await _enquiries.CountAsync(e => e.CreatedAt >= date && e.CreatedAt < nextDate, cancellationToken);
            var responded = await _enquiries.CountAsync(e => e.Status == EnquiryStatus.Responded && e.UpdatedAt >= date && e.UpdatedAt < nextDate, cancellationToken);
            var closed = await _enquiries.CountAsync(e => e.Status == EnquiryStatus.Closed && e.ClosedAt >= date && e.ClosedAt < nextDate, cancellationToken);

            result.Add(new EnquiryAnalyticsDto
            {
                Date = date,
                EnquiriesCreated = created,
                EnquiriesResponded = responded,
                EnquiriesClosed = closed
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<TestDriveAnalyticsDto>> GetTestDriveAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<TestDriveAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var requested = await _testDrives.CountAsync(td => td.CreatedAt >= date && td.CreatedAt < nextDate, cancellationToken);
            var confirmed = await _testDrives.CountAsync(td => td.Status == TestDriveStatus.Confirmed && td.UpdatedAt >= date && td.UpdatedAt < nextDate, cancellationToken);
            var completed = await _testDrives.CountAsync(td => td.Status == TestDriveStatus.Completed && td.UpdatedAt >= date && td.UpdatedAt < nextDate, cancellationToken);
            var cancelled = await _testDrives.CountAsync(td => td.Status == TestDriveStatus.Cancelled && td.UpdatedAt >= date && td.UpdatedAt < nextDate, cancellationToken);
            var noShow = await _testDrives.CountAsync(td => td.Status == TestDriveStatus.NoShow && td.UpdatedAt >= date && td.UpdatedAt < nextDate, cancellationToken);

            var conversionRate = completed + cancelled + noShow > 0
                ? (double)completed / (completed + cancelled + noShow) * 100
                : (double?)null;

            result.Add(new TestDriveAnalyticsDto
            {
                Date = date,
                Requested = requested,
                Confirmed = confirmed,
                Completed = completed,
                Cancelled = cancelled,
                NoShow = noShow,
                ConversionRate = conversionRate
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<ServiceAnalyticsDto>> GetServiceAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<ServiceAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var created = await _serviceBookings.CountAsync(sb => sb.CreatedAt >= date && sb.CreatedAt < nextDate, cancellationToken);
            var completed = await _serviceBookings.CountAsync(sb => sb.Status == ServiceBookingStatus.Completed && sb.CompletedAt >= date && sb.CompletedAt < nextDate, cancellationToken);
            var cancelled = await _serviceBookings.CountAsync(sb => sb.Status == ServiceBookingStatus.Cancelled && sb.CancelledAt >= date && sb.CancelledAt < nextDate, cancellationToken);

            result.Add(new ServiceAnalyticsDto
            {
                Date = date,
                BookingsCreated = created,
                BookingsCompleted = completed,
                BookingsCancelled = cancelled
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<AdvertisementAnalyticsDto>> GetAdvertisementAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<AdvertisementAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var impressions = await _adImpressions.CountAsync(ai => ai.OccurredAt >= date && ai.OccurredAt < nextDate, cancellationToken);
            var clicks = await _adClicks.CountAsync(ac => ac.OccurredAt >= date && ac.OccurredAt < nextDate, cancellationToken);
            var activeCampaigns = await _adCampaigns.CountAsync(ac => ac.Status == AdvertisementCampaignStatus.Running, cancellationToken);
            var completedCampaigns = await _adCampaigns.CountAsync(ac => ac.Status == AdvertisementCampaignStatus.Completed && ac.EndDate >= date && ac.EndDate < nextDate, cancellationToken);

            var ctr = impressions > 0 ? (double)clicks / impressions * 100 : (double?)null;

            result.Add(new AdvertisementAnalyticsDto
            {
                Date = date,
                Impressions = impressions,
                Clicks = clicks,
                CTR = ctr,
                ActiveCampaigns = activeCampaigns,
                CompletedCampaigns = completedCampaigns,
                Spend = 0,
                BudgetUtilization = null
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<SubscriptionAnalyticsDto>> GetSubscriptionAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<SubscriptionAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var active = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
            var newSubs = await _subscriptions.CountAsync(s => s.CreatedAt >= date && s.CreatedAt < nextDate, cancellationToken);
            var renewals = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Active && s.CurrentPeriodStart >= date && s.CurrentPeriodStart < nextDate, cancellationToken);
            var cancellations = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Cancelled && s.CancelledAt >= date && s.CancelledAt < nextDate, cancellationToken);
            var expired = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Expired && s.EndDate >= date && s.EndDate < nextDate, cancellationToken);

            result.Add(new SubscriptionAnalyticsDto
            {
                Date = date,
                ActiveSubscriptions = active,
                NewSubscriptions = newSubs,
                Renewals = renewals,
                Cancellations = cancellations,
                Expired = expired,
                SubscriptionRevenue = 0
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<SellVehicleAnalyticsDto>> GetSellVehicleAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<SellVehicleAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var submitted = await _sellRequests.CountAsync(sr => sr.CreatedAt >= date && sr.CreatedAt < nextDate, cancellationToken);
            var completed = await _sellRequests.CountAsync(sr => sr.Status == SellRequestStatus.Completed && sr.ClosedAt >= date && sr.ClosedAt < nextDate, cancellationToken);
            var cancelled = await _sellRequests.CountAsync(sr => sr.Status == SellRequestStatus.Cancelled && sr.ClosedAt >= date && sr.ClosedAt < nextDate, cancellationToken);
            var inspectionsCompleted = await _inspections.CountAsync(ins => ins.Status == InspectionStatus.Completed && ins.CompletedAt >= date && ins.CompletedAt < nextDate, cancellationToken);
            var offersCreated = await _sellOffers.CountAsync(so => so.CreatedAt >= date && so.CreatedAt < nextDate, cancellationToken);
            var offersAccepted = await _sellOffers.CountAsync(so => so.Status == SellOfferStatus.Accepted && so.AcceptedAt >= date && so.AcceptedAt < nextDate, cancellationToken);

            result.Add(new SellVehicleAnalyticsDto
            {
                Date = date,
                RequestsSubmitted = submitted,
                RequestsCompleted = completed,
                RequestsCancelled = cancelled,
                InspectionsCompleted = inspectionsCompleted,
                OffersCreated = offersCreated,
                OffersAccepted = offersAccepted
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<RevenueAnalyticsDto>> GetRevenueAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<RevenueAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var grossRevenue = await _payments.CountAsync(p => p.Status == PaymentStatus.Succeeded && p.CompletedAt >= date && p.CompletedAt < nextDate, cancellationToken);
            var refunds = await _refunds.CountAsync(pr => pr.Status == RefundStatus.Succeeded && pr.CompletedAt >= date && pr.CompletedAt < nextDate, cancellationToken);

            result.Add(new RevenueAnalyticsDto
            {
                Date = date,
                GrossRevenue = grossRevenue,
                Refunds = refunds,
                NetRevenue = grossRevenue - refunds,
                SubscriptionRevenue = 0,
                AdvertisementRevenue = 0,
                ListingPromotionRevenue = 0,
                OtherRevenue = grossRevenue
            });
        }

        return result;
    }

    public async Task<IReadOnlyList<PlatformAnalyticsDto>> GetPlatformAnalyticsAsync(AnalyticsQueryParams? parameters, CancellationToken cancellationToken = default)
    {
        var (from, to) = ResolveDateRange(parameters);
        var days = Math.Max(1, (to - from).Days);

        var result = new List<PlatformAnalyticsDto>();
        for (var i = 0; i < days; i++)
        {
            var date = from.AddDays(i);
            var nextDate = date.AddDays(1);

            var newUsers = await _users.CountAsync(u => u.CreatedAt >= date && u.CreatedAt < nextDate, cancellationToken);
            var activeUsers = await _users.CountAsync(u => u.IsActive, cancellationToken);
            var totalListings = await _listings.CountAsync(l => l.CreatedAt < nextDate, cancellationToken);
            var publishedListings = await _listings.CountAsync(l => l.Status == ListingStatus.Published, cancellationToken);
            var enquiries = await _enquiries.CountAsync(e => e.CreatedAt >= date && e.CreatedAt < nextDate, cancellationToken);
            var serviceBookings = await _serviceBookings.CountAsync(sb => sb.CreatedAt >= date && sb.CreatedAt < nextDate, cancellationToken);
            var activeAds = await _advertisements.CountAsync(a => a.Status == AdvertisementStatus.Active, cancellationToken);
            var activeSubs = await _subscriptions.CountAsync(s => s.Status == SubscriptionStatus.Active, cancellationToken);
            var sellRequests = await _sellRequests.CountAsync(sr => sr.CreatedAt >= date && sr.CreatedAt < nextDate, cancellationToken);

            var revenue = await _payments.CountAsync(p => p.Status == PaymentStatus.Succeeded && p.CompletedAt >= date && p.CompletedAt < nextDate, cancellationToken);

            result.Add(new PlatformAnalyticsDto
            {
                Date = date,
                NewUsers = newUsers,
                ActiveUsers = activeUsers,
                TotalListings = totalListings,
                PublishedListings = publishedListings,
                Enquiries = enquiries,
                ServiceBookings = serviceBookings,
                ActiveAdvertisements = activeAds,
                ActiveSubscriptions = activeSubs,
                SellRequests = sellRequests,
                Revenue = revenue
            });
        }

        return result;
    }

    private static (DateTime From, DateTime To) ResolveDateRange(AnalyticsQueryParams? parameters)
    {
        var to = parameters?.To ?? DateTime.UtcNow;
        var from = parameters?.From ?? to.AddDays(-30);
        return (from, to);
    }
}
