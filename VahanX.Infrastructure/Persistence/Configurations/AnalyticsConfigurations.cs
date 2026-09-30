using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ListingAnalytics.
/// </summary>
public class ListingAnalyticsConfiguration : BaseEntityConfiguration<ListingAnalytics>
{
    public override void Configure(EntityTypeBuilder<ListingAnalytics> builder)
    {
        base.Configure(builder);
        builder.ToTable("ListingAnalytics");
        builder.HasIndex(e => e.ListingId).IsUnique();
        builder.HasIndex(e => e.LastViewedAt);
    }
}

/// <summary>
/// Entity configuration for SearchAnalytics.
/// </summary>
public class SearchAnalyticsConfiguration : BaseEntityConfiguration<SearchAnalytics>
{
    public override void Configure(EntityTypeBuilder<SearchAnalytics> builder)
    {
        base.Configure(builder);
        builder.ToTable("SearchAnalytics");
        builder.Property(e => e.SearchQuery).HasMaxLength(500).IsRequired();
        builder.Property(e => e.Filters).HasMaxLength(2000).IsRequired(false);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.UserId);
    }
}

/// <summary>
/// Entity configuration for PlatformAnalytics.
/// </summary>
public class PlatformAnalyticsConfiguration : BaseEntityConfiguration<PlatformAnalytics>
{
    public override void Configure(EntityTypeBuilder<PlatformAnalytics> builder)
    {
        base.Configure(builder);
        builder.ToTable("PlatformAnalytics");
        builder.Property(e => e.Revenue).HasPrecision(18, 2);
        builder.HasIndex(e => e.Date).IsUnique();
    }
}
