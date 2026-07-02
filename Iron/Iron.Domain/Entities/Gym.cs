using Iron.Domain.ValueObjects;

namespace Iron.Domain.Entities;

public class Gym : BaseEntity
{
    public string TradeName { get; private set; } = string.Empty;
    public string CorporateName { get; private set; } = string.Empty;
    public Cnpj Cnpj { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public long PlanId { get; private set; }
    public Plan Plan { get; private set; } = null!;
    public ICollection<Membership> Memberships { get; private set; } = [];

    private Gym() { }

    private Gym(string tradeName, string corporateName, Cnpj cnpj, Email email, PhoneNumber phoneNumber, long planId)
    {
        CreatedAt = DateTime.UtcNow;
        TradeName = tradeName;
        CorporateName = corporateName;
        Cnpj = cnpj;
        Email = email;
        PhoneNumber = phoneNumber;
        PlanId = planId;
        IsActive = true;
    }

    public static Gym Create(string tradeName, string corporateName, Cnpj cnpj, Email email, PhoneNumber phoneNumber, long planId)
    {
        if (string.IsNullOrWhiteSpace(tradeName))
            throw new ArgumentException("Nome fantasia não pode ser vazio.", nameof(tradeName));

        if (string.IsNullOrWhiteSpace(corporateName))
            throw new ArgumentException("Razão social não pode ser vazia.", nameof(corporateName));

        if (planId <= 0)
            throw new ArgumentOutOfRangeException(nameof(planId), "Plano inválido.");

        ArgumentNullException.ThrowIfNull(cnpj);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return new Gym(tradeName, corporateName, cnpj, email, phoneNumber, planId);
    }

    public void Rename(string tradeName, string corporateName)
    {
        if (string.IsNullOrWhiteSpace(tradeName))
            throw new ArgumentException("Nome fantasia não pode ser vazio.", nameof(tradeName));

        if (string.IsNullOrWhiteSpace(corporateName))
            throw new ArgumentException("Razão social não pode ser vazia.", nameof(corporateName));

        TradeName = tradeName;
        CorporateName = corporateName;
        Touch();
    }

    public void UpdateContactInfo(Email email, PhoneNumber phoneNumber)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(phoneNumber);

        Email = email;
        PhoneNumber = phoneNumber;
        Touch();
    }

    public void ChangePlan(long planId)
    {
        if (planId <= 0)
            throw new ArgumentOutOfRangeException(nameof(planId), "Plano inválido.");

        PlanId = planId;
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
