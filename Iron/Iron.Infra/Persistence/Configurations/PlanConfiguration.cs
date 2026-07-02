using Iron.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iron.Infrastructure.Persistence.Configurations;

public class PlanConfiguration : BaseEntityConfiguration<Plan>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.MonthlyPrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.MaxStudents).IsRequired();
        builder.Property(p => p.MaxEmployees).IsRequired();
        builder.Property(p => p.IsActive).IsRequired();
    }
}
