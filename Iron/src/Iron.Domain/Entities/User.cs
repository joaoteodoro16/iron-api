using Iron.Domain.ValueObjects;

namespace Iron.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public bool EmailConfirmed { get; private set; }
    public bool IsPlatformAdmin { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public ICollection<Membership> Memberships { get; private set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

    private User() { }

    private User(string firstName, string lastName, Email email, string passwordHash, PhoneNumber phoneNumber, bool isPlatformAdmin)
    {
        CreatedAt = DateTime.UtcNow;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        IsPlatformAdmin = isPlatformAdmin;
        IsActive = true;
    }

    public static User Create(string firstName, string lastName, Email email, string passwordHash, PhoneNumber phoneNumber, bool isPlatformAdmin = false)
    {
        ValidateName(firstName, lastName);
        ValidatePasswordHash(passwordHash);

        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return new User(firstName, lastName, email, passwordHash, phoneNumber, isPlatformAdmin);
    }

    public void UpdateProfile(string firstName, string lastName, PhoneNumber phoneNumber)
    {
        ValidateName(firstName, lastName);

        ArgumentNullException.ThrowIfNull(phoneNumber);

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Touch();
    }

    public void ChangeEmail(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        Email = email;
        EmailConfirmed = false;
        Touch();
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        Touch();
    }

    public void ChangePassword(string passwordHash)
    {
        ValidatePasswordHash(passwordHash);

        PasswordHash = passwordHash;
        Touch();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
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

    public void GrantPlatformAdmin()
    {
        IsPlatformAdmin = true;
        Touch();
    }

    public void RevokePlatformAdmin()
    {
        IsPlatformAdmin = false;
        Touch();
    }

    private static void ValidateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Sobrenome não pode ser vazio.", nameof(lastName));
    }

    private static void ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Hash de senha não pode ser vazio.", nameof(passwordHash));
    }
}
