using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence.Configurations;

public class PageModelConfiguration : IEntityTypeConfiguration<PageModel>
{
    public void Configure(EntityTypeBuilder<PageModel> builder)
    {
        builder.ToTable("PageModels");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Icon).HasMaxLength(100);

        builder.Property(x => x.SchemaJson).HasColumnType("jsonb").IsRequired();

        builder.HasMany(x => x.PageInstances)
               .WithOne(x => x.PageModel)
               .HasForeignKey(x => x.PageModelId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
