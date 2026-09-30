using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for InspectionReport.
/// </summary>
public class InspectionReportConfiguration : BaseEntityConfiguration<InspectionReport>
{
    public override void Configure(EntityTypeBuilder<InspectionReport> builder)
    {
        base.Configure(builder);

        builder.ToTable("InspectionReports");

        builder.Property(e => e.OverallScore)
            .HasPrecision(5, 2)
            .IsRequired(false);

        builder.Property(e => e.Summary)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Recommendations)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.ReportReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.ReportStatus)
            .IsRequired();

        builder.HasOne(e => e.Inspection)
            .WithOne(e => e.Report)
            .HasForeignKey<InspectionReport>(e => e.InspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.InspectionId)
            .IsUnique();
    }
}
