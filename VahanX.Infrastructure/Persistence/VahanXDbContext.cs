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
