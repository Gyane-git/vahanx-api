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
