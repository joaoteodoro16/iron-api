using Iron.Domain.Entities;
using Iron.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class TrainerConfiguration : BaseEntityConfiguration<Trainer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Trainer> builder)
    {
        builder.Property(t => t.Cref)
            .HasConversion(cref => cref.Value, value => Cref.Create(value))
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(t => t.HireDate).IsRequired();

        builder.HasIndex(t => t.UserId)
            .IsUnique();

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
