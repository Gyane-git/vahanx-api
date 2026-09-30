namespace VahanX.Application.DTOs.Admin;

/// <summary>
/// Admin dashboard summary DTO.
/// </summary>
public class AdminDashboardSummaryDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int NewUsersToday { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int TotalListings { get; set; }
    public int PublishedListings { get; set; }
    public int PendingListings { get; set; }
    public int SoldListings { get; set; }
    public int TotalSellers { get; set; }
    public int TotalDealers { get; set; }
    public int PendingVerifications { get; set; }
    public int PendingInspections { get; set; }
    public int PendingReports { get; set; }
    public int PendingModerationCases { get; set; }
    public int ActiveAdvertisements { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int ServiceBookingsToday { get; set; }
    public int SellRequestsOpen { get; set; }
    public decimal RevenueToday { get; set; }
    public decimal RevenueThisMonth { get; set; }
}

/// <summary>
/// Admin permission constants.
/// </summary>
public static class AdminPermissions
{
    public const string UserView = "USER_VIEW";
    public const string UserManage = "USER_MANAGE";
    public const string UserSuspend = "USER_SUSPEND";
    public const string ListingView = "LISTING_VIEW";
    public const string ListingModerate = "LISTING_MODERATE";
    public const string ListingApprove = "LISTING_APPROVE";
    public const string ListingReject = "LISTING_REJECT";
    public const string SellerView = "SELLER_VIEW";
    public const string SellerVerify = "SELLER_VERIFY";
    public const string SellerSuspend = "SELLER_SUSPEND";
    public const string DealerView = "DEALER_VIEW";
    public const string DealerVerify = "DEALER_VERIFY";
    public const string DealerSuspend = "DEALER_SUSPEND";
    public const string ReviewModerate = "REVIEW_MODERATE";
    public const string AdvertisementModerate = "ADVERTISEMENT_MODERATE";
    public const string VerificationReview = "VERIFICATION_REVIEW";
    public const string InspectionReview = "INSPECTION_REVIEW";
    public const string CmsView = "CMS_VIEW";
    public const string CmsManage = "CMS_MANAGE";
    public const string AnalyticsView = "ANALYTICS_VIEW";
    public const string AnalyticsRevenueView = "ANALYTICS_REVENUE_VIEW";
    public const string AnalyticsUserView = "ANALYTICS_USER_VIEW";
    public const string AnalyticsAdView = "ANALYTICS_AD_VIEW";
    public const string AuditView = "AUDIT_VIEW";
    public const string SystemConfigView = "SYSTEM_CONFIG_VIEW";
    public const string SystemConfigManage = "SYSTEM_CONFIG_MANAGE";
}

/// <summary>
/// Admin role examples (database-driven, not hardcoded).
/// </summary>
public static class AdminRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Moderator = "Moderator";
    public const string ContentManager = "ContentManager";
    public const string AnalyticsViewer = "AnalyticsViewer";
    public const string AuditViewer = "AuditViewer";
    public const string SupportAgent = "SupportAgent";
}
