namespace Iron.Domain.Entities;

public class Plan : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public decimal MonthlyPrice { get; private set; }

    public int MaxStudents { get; private set; }

    public int MaxEmployees { get; private set; }

    public bool IsActive { get; private set; }

    public ICollection<Gym> Tenants { get; private set; } = [];

    private Plan() { }

    private Plan(string name, decimal monthlyPrice, int maxStudents, int maxEmployees)
    {
        CreatedAt = DateTime.UtcNow;
        Name = name;
        MonthlyPrice = monthlyPrice;
        MaxStudents = maxStudents;
        MaxEmployees = maxEmployees;
        IsActive = true;
    }

    public static Plan Create(string name, decimal monthlyPrice, int maxStudents, int maxEmployees)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do plano não pode ser vazio.", nameof(name));

        if (monthlyPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyPrice), "Preço mensal não pode ser negativo.");

        if (maxStudents <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxStudents), "Limite de alunos deve ser maior que zero.");

        if (maxEmployees <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxEmployees), "Limite de funcionários deve ser maior que zero.");

        return new Plan(name, monthlyPrice, maxStudents, maxEmployees);
    }

    public void UpdatePricing(decimal monthlyPrice)
    {
        if (monthlyPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyPrice), "Preço mensal não pode ser negativo.");

        MonthlyPrice = monthlyPrice;
        Touch();
    }

    public void UpdateLimits(int maxStudents, int maxEmployees)
    {
        if (maxStudents <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxStudents), "Limite de alunos deve ser maior que zero.");

        if (maxEmployees <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxEmployees), "Limite de funcionários deve ser maior que zero.");

        MaxStudents = maxStudents;
        MaxEmployees = maxEmployees;
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
