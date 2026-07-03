using Iron.Domain.Entities;
using Iron.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class GymConfiguration : BaseEntityConfiguration<Gym>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Gym> builder)
    {
        builder.Property(g => g.TradeName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(g => g.CorporateName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(g => g.Cnpj)
            .HasConversion(cnpj => cnpj.Value, value => Cnpj.Create(value))
            .HasMaxLength(14)
            .IsRequired();

        builder.HasIndex(g => g.Cnpj)
            .IsUnique();

        builder.Property(g => g.Email)
            .HasConversion(email => email.Value, value => Email.Create(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(g => g.PhoneNumber)
            .HasConversion(phone => phone.Value, value => PhoneNumber.Create(value))
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(g => g.IsActive).IsRequired();

        builder.HasOne(g => g.Plan)
            .WithMany(p => p.Tenants)
            .HasForeignKey(g => g.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
