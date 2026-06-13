using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence.Configurations;

public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("AppSettings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.GeneralJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.SeoJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.SmtpJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.LocalizationJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.ThemeJson).HasColumnType("jsonb").IsRequired();

        // Seed the singleton row so the app always has a valid record to read/update.
        builder.HasData(new AppSetting
        {
            Id = AppSetting.SingletonId,
            GeneralJson = "{}",
            SeoJson = "{}",
            SmtpJson = "{}",
            LocalizationJson = "{}",
            ThemeJson = "{}",
            UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
