using Iron.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class MembershipConfiguration : BaseEntityConfiguration<Membership>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Membership> builder)
    {
        builder.Property(m => m.IsActive).IsRequired();
        builder.Property(m => m.JoinedAt).IsRequired();

        builder.HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Gym)
            .WithMany(g => g.Memberships)
            .HasForeignKey(m => m.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Role)
            .WithMany(r => r.Memberships)
            .HasForeignKey(m => m.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
