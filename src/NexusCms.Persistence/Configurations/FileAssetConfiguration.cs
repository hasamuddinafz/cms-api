using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence.Configurations;

public class FileAssetConfiguration : IEntityTypeConfiguration<FileAsset>
{
    public void Configure(EntityTypeBuilder<FileAsset> builder)
    {
        builder.ToTable("FileAssets");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).HasMaxLength(500).IsRequired();
        builder.Property(x => x.OriginalFileName).HasMaxLength(500).IsRequired();
        builder.Property(x => x.MimeType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Url).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ThumbnailUrl).HasMaxLength(1000);
        builder.Property(x => x.Folder).HasMaxLength(200);
        builder.Property(x => x.FileType).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.AltTextJson).HasColumnType("jsonb").IsRequired();

        builder.HasIndex(x => x.FileType);
        builder.HasIndex(x => x.CreatedAt);
    }
}
