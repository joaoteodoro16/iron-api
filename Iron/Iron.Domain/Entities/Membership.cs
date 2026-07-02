namespace Iron.Domain.Entities;

public class Membership : BaseEntity
{
    public long UserId { get; private set; }

    public long TenantId { get; private set; }

    public long RoleId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime JoinedAt { get; private set; }

    public User User { get; private set; } = null!;

    public Gym Gym { get; private set; } = null!;

    public Role Role { get; private set; } = null!;

    private Membership() { }

    private Membership(long userId, long tenantId, long roleId)
    {
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        TenantId = tenantId;
        RoleId = roleId;
        JoinedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public static Membership Create(long userId, long tenantId, long roleId)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (tenantId <= 0)
            throw new ArgumentOutOfRangeException(nameof(tenantId));

        if (roleId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roleId));

        return new Membership(userId, tenantId, roleId);
    }

    public void ChangeRole(long roleId)
    {
        if (roleId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roleId));

        RoleId = roleId;
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }
}
