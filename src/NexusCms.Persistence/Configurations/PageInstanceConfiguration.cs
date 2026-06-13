using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence.Configurations;

public class PageInstanceConfiguration : IEntityTypeConfiguration<PageInstance>
{
    public void Configure(EntityTypeBuilder<PageInstance> builder)
    {
        builder.ToTable("PageInstances");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Slug).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.PageModelId, x.Slug }).IsUnique();

        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.ContentJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.SeoMetaJson).HasColumnType("jsonb").IsRequired();

        builder.HasMany(x => x.ViewLogs)
               .WithOne(x => x.PageInstance)
               .HasForeignKey(x => x.PageInstanceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
