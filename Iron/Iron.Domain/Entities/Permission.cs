namespace Iron.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public ICollection<RolePermission> RolePermissions { get; private set; } = [];

    private Permission() { }

    private Permission(string name, string description)
    {
        CreatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
    }

    public static Permission Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da permissão não pode ser vazio.", nameof(name));

        return new Permission(name, description);
    }

    public void UpdateDescription(string description)
    {
        Description = description;
        Touch();
    }
}
