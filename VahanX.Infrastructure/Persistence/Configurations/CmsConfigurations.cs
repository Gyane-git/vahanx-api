using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ContentCategory.
/// </summary>
public class ContentCategoryConfiguration : BaseEntityConfiguration<ContentCategory>
{
    public override void Configure(EntityTypeBuilder<ContentCategory> builder)
    {
        base.Configure(builder);
        builder.ToTable("ContentCategories");
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Slug).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
        builder.HasIndex(e => e.Slug).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}

/// <summary>
/// Entity configuration for ContentTag.
/// </summary>
public class ContentTagConfiguration : BaseEntityConfiguration<ContentTag>
{
    public override void Configure(EntityTypeBuilder<ContentTag> builder)
    {
        base.Configure(builder);
        builder.ToTable("ContentTags");
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Slug).HasMaxLength(100).IsRequired();
        builder.HasIndex(e => e.Slug).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}

/// <summary>
/// Entity configuration for Page.
/// </summary>
public class PageConfiguration : BaseEntityConfiguration<Page>
{
    public override void Configure(EntityTypeBuilder<Page> builder)
    {
        base.Configure(builder);
        builder.ToTable("Pages");
        builder.Property(e => e.Slug).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Title).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Summary).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.Content).IsRequired();
        builder.Property(e => e.SeoTitle).HasMaxLength(300).IsRequired(false);
        builder.Property(e => e.SeoDescription).HasMaxLength(500).IsRequired(false);
        builder.HasIndex(e => e.Slug).IsUnique();
        builder.HasIndex(e => e.Status);
    }
}

/// <summary>
/// Entity configuration for Article.
/// </summary>
public class ArticleConfiguration : BaseEntityConfiguration<Article>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);
        builder.ToTable("Articles");
        builder.Property(e => e.Slug).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Title).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Summary).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.Content).IsRequired();
        builder.Property(e => e.SeoTitle).HasMaxLength(300).IsRequired(false);
        builder.Property(e => e.SeoDescription).HasMaxLength(500).IsRequired(false);
        builder.HasOne<ContentCategory>().WithMany().HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(e => e.Slug).IsUnique();
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CategoryId);
        builder.HasIndex(e => e.PublishedAt);
    }
}

/// <summary>
/// Entity configuration for FAQ.
/// </summary>
public class FaqConfiguration : BaseEntityConfiguration<FAQ>
{
    public override void Configure(EntityTypeBuilder<FAQ> builder)
    {
        base.Configure(builder);
        builder.ToTable("FAQs");
        builder.Property(e => e.Question).HasMaxLength(500).IsRequired();
        builder.Property(e => e.Answer).IsRequired();
        builder.HasOne<ContentCategory>().WithMany().HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(e => e.CategoryId);
        builder.HasIndex(e => e.IsPublished);
    }
}

/// <summary>
/// Entity configuration for Banner.
/// </summary>
public class BannerConfiguration : BaseEntityConfiguration<Banner>
{
    public override void Configure(EntityTypeBuilder<Banner> builder)
    {
        base.Configure(builder);
        builder.ToTable("Banners");
        builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Subtitle).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.MediaReference).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.MobileMediaReference).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.DesktopMediaReference).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.Placement).HasMaxLength(100).IsRequired();
        builder.Property(e => e.TargetReference).HasMaxLength(500).IsRequired(false);
        builder.HasIndex(e => e.Placement);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.StartAt);
        builder.HasIndex(e => e.EndAt);
    }
}

/// <summary>
/// Entity configuration for Promotion.
/// </summary>
public class PromotionConfiguration : BaseEntityConfiguration<Promotion>
{
    public override void Configure(EntityTypeBuilder<Promotion> builder)
    {
        base.Configure(builder);
        builder.ToTable("Promotions");
        builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(2000).IsRequired();
        builder.Property(e => e.MediaReference).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.TargetReference).HasMaxLength(500).IsRequired(false);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.StartAt);
        builder.HasIndex(e => e.EndAt);
    }
}

/// <summary>
/// Entity configuration for Announcement.
/// </summary>
public class AnnouncementConfiguration : BaseEntityConfiguration<Announcement>
{
    public override void Configure(EntityTypeBuilder<Announcement> builder)
    {
        base.Configure(builder);
        builder.ToTable("Announcements");
        builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Content).IsRequired();
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex(e => e.StartAt);
        builder.HasIndex(e => e.EndAt);
    }
}

/// <summary>
/// Entity configuration for ContentMedia.
/// </summary>
public class ContentMediaConfiguration : BaseEntityConfiguration<ContentMedia>
{
    public override void Configure(EntityTypeBuilder<ContentMedia> builder)
    {
        base.Configure(builder);
        builder.ToTable("ContentMedia");
        builder.Property(e => e.ContentType).HasMaxLength(50).IsRequired();
        builder.Property(e => e.MediaReference).HasMaxLength(500).IsRequired();
        builder.Property(e => e.Caption).HasMaxLength(500).IsRequired(false);
        builder.HasIndex(e => new { e.ContentId, e.ContentType });
    }
}
