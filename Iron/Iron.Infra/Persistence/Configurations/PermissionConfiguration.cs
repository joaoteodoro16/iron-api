using Iron.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(255);
    }
}
