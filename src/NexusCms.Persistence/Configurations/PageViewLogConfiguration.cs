using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence.Configurations;

public class PageViewLogConfiguration : IEntityTypeConfiguration<PageViewLog>
{
    public void Configure(EntityTypeBuilder<PageViewLog> builder)
    {
        builder.ToTable("PageViewLogs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpHash).HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.Referrer).HasMaxLength(1000);
        builder.Property(x => x.CountryCode).HasMaxLength(5);

        builder.HasIndex(x => x.PageInstanceId);
        builder.HasIndex(x => x.ViewedAt);
    }
}
