using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceCenter.
/// </summary>
public class ServiceCenterConfiguration : BaseEntityConfiguration<ServiceCenter>
{
    public override void Configure(EntityTypeBuilder<ServiceCenter> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceCenters");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.BusinessRegistrationNumber)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.ContactEmail)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.WebsiteUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.LogoMediaReference)
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(e => e.VerificationStatus)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Dealer)
            .WithMany()
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.VerificationStatus);
    }
}
