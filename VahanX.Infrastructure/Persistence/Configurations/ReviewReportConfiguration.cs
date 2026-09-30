using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ReviewReport.
/// </summary>
public class ReviewReportConfiguration : BaseEntityConfiguration<ReviewReport>
{
    public override void Configure(EntityTypeBuilder<ReviewReport> builder)
    {
        base.Configure(builder);

        builder.ToTable("ReviewReports");

        builder.Property(e => e.Reason)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.HasIndex(e => e.ReviewId);
        builder.HasIndex(e => e.ReportedBy);
    }
}
