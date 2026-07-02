using Iron.Domain.Entities;
using Iron.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(email => email.Value, value => Email.Create(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .HasConversion(phone => phone.Value, value => PhoneNumber.Create(value))
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(u => u.EmailConfirmed).IsRequired();
        builder.Property(u => u.IsPlatformAdmin).IsRequired();
        builder.Property(u => u.IsActive).IsRequired();
    }
}
