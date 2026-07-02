namespace Iron.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    //Define que a role é do sistema e nao pode ser excluida por exemplo
    public bool IsSystemRole { get; private set; }

    public ICollection<RolePermission> RolePermissions { get; private set; } = [];

    public ICollection<Membership> Memberships { get; private set; } = [];

    private Role() { }

    private Role(string name, string description, bool isSystemRole)
    {
        CreatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
        IsSystemRole = isSystemRole;
    }

    public static Role Create(string name, string description, bool isSystemRole = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da regra não pode ser vazio.", nameof(name));

        return new Role(name, description, isSystemRole);
    }

    public void Rename(string name, string description)
    {
        if (IsSystemRole)
            throw new InvalidOperationException("Regras do sistema não podem ser renomeadas.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da regra não pode ser vazio.", nameof(name));

        Name = name;
        Description = description;
        Touch();
    }
}
