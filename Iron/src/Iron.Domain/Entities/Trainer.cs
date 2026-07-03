using Iron.Domain.ValueObjects;

namespace Iron.Domain.Entities;

public class Trainer : BaseEntity
{
    public long UserId { get; private set; }

    public Cref Cref { get; private set; } = null!;

    public DateOnly HireDate { get; private set; }

    public User User { get; private set; } = null!;

    private Trainer() { }

    private Trainer(long userId, Cref cref, DateOnly hireDate)
    {
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        Cref = cref;
        HireDate = hireDate;
    }

    public static Trainer Create(long userId, Cref cref, DateOnly hireDate)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        ArgumentNullException.ThrowIfNull(cref);

        return new Trainer(userId, cref, hireDate);
    }

    public void UpdateCref(Cref cref)
    {
        ArgumentNullException.ThrowIfNull(cref);

        Cref = cref;
        Touch();
    }
}
