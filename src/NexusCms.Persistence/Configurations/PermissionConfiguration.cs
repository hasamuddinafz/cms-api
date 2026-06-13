using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Module).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Label).HasMaxLength(200).IsRequired();

        builder.HasMany(x => x.RolePermissions)
               .WithOne(x => x.Permission)
               .HasForeignKey(x => x.PermissionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
