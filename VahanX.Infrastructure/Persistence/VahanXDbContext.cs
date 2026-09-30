using Microsoft.EntityFrameworkCore;
using VahanX.Domain.Common;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence;

/// <summary>
/// Main database context for VahanX.
/// </summary>
public class VahanXDbContext : DbContext
{
    public VahanXDbContext(DbContextOptions<VahanXDbContext> options)
        : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // Identity
    public DbSet<User> Users => Set<User>();

    // Vehicle Core
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Model> Models => Set<Model>();
    public DbSet<Generation> Generations => Set<Generation>();
    public DbSet<Variant> Variants => Set<Variant>();
    public DbSet<BodyType> BodyTypes => Set<BodyType>();
    public DbSet<FuelType> FuelTypes => Set<FuelType>();
    public DbSet<TransmissionType> TransmissionTypes => Set<TransmissionType>();
    public DbSet<VahanX.Domain.Entities.DriveType> DriveTypes => Set<VahanX.Domain.Entities.DriveType>();
    public DbSet<EngineType> EngineTypes => Set<EngineType>();
    public DbSet<VehicleFeature> VehicleFeatures => Set<VehicleFeature>();
    public DbSet<VehicleSpecification> VehicleSpecifications => Set<VehicleSpecification>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    // Marketplace
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Seller> Sellers => Set<Seller>();
    public DbSet<Dealer> Dealers => Set<Dealer>();
    public DbSet<DealerBranch> DealerBranches => Set<DealerBranch>();
    public DbSet<DealerStaff> DealerStaff => Set<DealerStaff>();
    public DbSet<VehicleListing> VehicleListings => Set<VehicleListing>();
    public DbSet<ListingMedia> ListingMedia => Set<ListingMedia>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<CompareList> CompareLists => Set<CompareList>();
    public DbSet<CompareItem> CompareItems => Set<CompareItem>();

    // Trust
    public DbSet<VehicleVerification> VehicleVerifications => Set<VehicleVerification>();
    public DbSet<VerificationDocument> VerificationDocuments => Set<VerificationDocument>();
    public DbSet<VehicleVerificationStatusHistory> VehicleVerificationStatusHistory => Set<VehicleVerificationStatusHistory>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<InspectionItem> InspectionItems => Set<InspectionItem>();
    public DbSet<InspectionReport> InspectionReports => Set<InspectionReport>();
    public DbSet<InspectionMedia> InspectionMedia => Set<InspectionMedia>();
    public DbSet<OwnershipHistory> OwnershipHistory => Set<OwnershipHistory>();
    public DbSet<MileageHistory> MileageHistory => Set<MileageHistory>();
    public DbSet<ServiceHistory> ServiceHistory => Set<ServiceHistory>();
    public DbSet<AccidentHistory> AccidentHistory => Set<AccidentHistory>();
    public DbSet<InsuranceHistory> InsuranceHistory => Set<InsuranceHistory>();
    public DbSet<RegistrationHistory> RegistrationHistory => Set<RegistrationHistory>();
    public DbSet<PriceHistory> PriceHistory => Set<PriceHistory>();
    public DbSet<VehicleReview> VehicleReviews => Set<VehicleReview>();
    public DbSet<SellerReview> SellerReviews => Set<SellerReview>();
    public DbSet<DealerReview> DealerReviews => Set<DealerReview>();
    public DbSet<ReviewReport> ReviewReports => Set<ReviewReport>();

    // Engagement
    public DbSet<Enquiry> Enquiries => Set<Enquiry>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MessageAttachment> MessageAttachments => Set<MessageAttachment>();
    public DbSet<TestDrive> TestDrives => Set<TestDrive>();
    public DbSet<TestDriveSlot> TestDriveSlots => Set<TestDriveSlot>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();
    public DbSet<PushToken> PushTokens => Set<PushToken>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

    // Services & Charging
    public DbSet<ServiceCenter> ServiceCenters => Set<ServiceCenter>();
    public DbSet<ServiceCenterBranch> ServiceCenterBranches => Set<ServiceCenterBranch>();
    public DbSet<AutoServiceType> AutoServiceTypes => Set<AutoServiceType>();
    public DbSet<ServiceCenterService> ServiceCenterServices => Set<ServiceCenterService>();
    public DbSet<ServicePackage> ServicePackages => Set<ServicePackage>();
    public DbSet<ServicePackageItem> ServicePackageItems => Set<ServicePackageItem>();
    public DbSet<ServiceWorkingHour> ServiceWorkingHours => Set<ServiceWorkingHour>();
    public DbSet<ServiceBooking> ServiceBookings => Set<ServiceBooking>();
    public DbSet<ChargingStation> ChargingStations => Set<ChargingStation>();
    public DbSet<ChargingStationConnector> ChargingStationConnectors => Set<ChargingStationConnector>();
    public DbSet<ChargingStationAmenity> ChargingStationAmenities => Set<ChargingStationAmenity>();
    public DbSet<ChargingStationAvailability> ChargingStationAvailability => Set<ChargingStationAvailability>();
    public DbSet<ChargingStationPrice> ChargingStationPrices => Set<ChargingStationPrice>();
    public DbSet<FuelStation> FuelStations => Set<FuelStation>();
    public DbSet<FuelStationFuelType> FuelStationFuelTypes => Set<FuelStationFuelType>();
    public DbSet<FuelStationAmenity> FuelStationAmenities => Set<FuelStationAmenity>();
    public DbSet<FuelStationAvailability> FuelStationAvailability => Set<FuelStationAvailability>();
    public DbSet<FuelStationPrice> FuelStationPrices => Set<FuelStationPrice>();
    public DbSet<FuelStationWorkingHour> FuelStationWorkingHours => Set<FuelStationWorkingHour>();
    public DbSet<ServiceReview> ServiceReviews => Set<ServiceReview>();

    // Business & Monetization
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<SubscriptionPlanFeature> SubscriptionPlanFeatures => Set<SubscriptionPlanFeature>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionUsage> SubscriptionUsage => Set<SubscriptionUsage>();
    public DbSet<SubscriptionChangeHistory> SubscriptionChangeHistory => Set<SubscriptionChangeHistory>();
    public DbSet<AdvertisementCampaign> AdvertisementCampaigns => Set<AdvertisementCampaign>();
    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
    public DbSet<AdvertisementCreative> AdvertisementCreatives => Set<AdvertisementCreative>();
    public DbSet<AdvertisementPlacement> AdvertisementPlacements => Set<AdvertisementPlacement>();
    public DbSet<AdvertisementTargeting> AdvertisementTargeting => Set<AdvertisementTargeting>();
    public DbSet<AdvertisementBudget> AdvertisementBudgets => Set<AdvertisementBudget>();
    public DbSet<AdvertisementImpression> AdvertisementImpressions => Set<AdvertisementImpression>();
    public DbSet<AdvertisementClick> AdvertisementClicks => Set<AdvertisementClick>();
    public DbSet<SellRequest> SellRequests => Set<SellRequest>();
    public DbSet<SellVehicle> SellVehicles => Set<SellVehicle>();
    public DbSet<SellOffer> SellOffers => Set<SellOffer>();
    public DbSet<SellRequestStatusHistory> SellRequestStatusHistory => Set<SellRequestStatusHistory>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentItem> PaymentItems => Set<PaymentItem>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<PaymentRefund> PaymentRefunds => Set<PaymentRefund>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    // Phase 9: Moderation
    public DbSet<ReportReason> ReportReasons => Set<ReportReason>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<ModerationCase> ModerationCases => Set<ModerationCase>();
    public DbSet<ModerationAction> ModerationActions => Set<ModerationAction>();
    public DbSet<ModerationHistory> ModerationHistory => Set<ModerationHistory>();
    public DbSet<UserRestriction> UserRestrictions => Set<UserRestriction>();

    // Phase 9: Analytics
    public DbSet<ListingAnalytics> ListingAnalytics => Set<ListingAnalytics>();
    public DbSet<SearchAnalytics> SearchAnalytics => Set<SearchAnalytics>();
    public DbSet<PlatformAnalytics> PlatformAnalytics => Set<PlatformAnalytics>();

    // Phase 9: CMS
    public DbSet<ContentCategory> ContentCategories => Set<ContentCategory>();
    public DbSet<ContentTag> ContentTags => Set<ContentTag>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<FAQ> FAQs => Set<FAQ>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<ContentMedia> ContentMedia => Set<ContentMedia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VahanXDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
