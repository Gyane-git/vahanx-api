using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for CompareList.
/// </summary>
public class CompareListConfiguration : BaseEntityConfiguration<CompareList>
{
    public override void Configure(EntityTypeBuilder<CompareList> builder)
    {
        base.Configure(builder);

        builder.ToTable("CompareLists");

        builder.HasIndex(e => e.UserId)
            .IsUnique();
    }
}
