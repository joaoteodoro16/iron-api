using Iron.Domain.Entities;
using Iron.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : BaseEntityConfiguration<Student>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Student> builder)
    {
        builder.Property(s => s.BirthDate).IsRequired();

        builder.Property(s => s.Weight)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(s => s.Height)
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(s => s.BloodType)
            .HasMaxLength(3);

        builder.Property(s => s.MedicalNotes)
            .HasMaxLength(1000);

        builder.Property(s => s.EmergencyContact)
            .HasConversion(phone => phone!.Value, value => PhoneNumber.Create(value))
            .HasMaxLength(11);

        builder.Property(s => s.EmergencyPhone)
            .HasConversion(phone => phone!.Value, value => PhoneNumber.Create(value))
            .HasMaxLength(11);

        builder.HasIndex(s => s.UserId)
            .IsUnique();

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
