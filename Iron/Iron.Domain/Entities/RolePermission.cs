namespace Iron.Domain.Entities;

public class RolePermission
{
    public long RoleId { get; private set; }

    public long PermissionId { get; private set; }

    public Role Role { get; private set; } = null!;

    public Permission Permission { get; private set; } = null!;

    private RolePermission() { }

    private RolePermission(long roleId, long permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static RolePermission Create(long roleId, long permissionId)
    {
        if (roleId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roleId));

        if (permissionId <= 0)
            throw new ArgumentOutOfRangeException(nameof(permissionId));

        return new RolePermission(roleId, permissionId);
    }
}
