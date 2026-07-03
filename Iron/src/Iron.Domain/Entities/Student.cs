using Iron.Domain.ValueObjects;

namespace Iron.Domain.Entities;

public class Student : BaseEntity
{
    public long UserId { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public decimal Weight { get; private set; }

    public decimal Height { get; private set; }

    public string? BloodType { get; private set; }

    public PhoneNumber? EmergencyContact { get; private set; }

    public PhoneNumber? EmergencyPhone { get; private set; }

    public string? MedicalNotes { get; private set; }

    public User User { get; private set; } = null!;

    private Student() { }

    private Student(long userId, DateOnly birthDate, decimal weight, decimal height, string? bloodType, PhoneNumber? emergencyContact, PhoneNumber? emergencyPhone, string? medicalNotes)
    {
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        BirthDate = birthDate;
        Weight = weight;
        Height = height;
        BloodType = bloodType;
        EmergencyContact = emergencyContact;
        EmergencyPhone = emergencyPhone;
        MedicalNotes = medicalNotes;
    }

    public static Student Create(
        long userId,
        DateOnly birthDate,
        decimal weight,
        decimal height,
        string? bloodType = null,
        PhoneNumber? emergencyContact = null,
        PhoneNumber? emergencyPhone = null,
        string? medicalNotes = null)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (birthDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentOutOfRangeException(nameof(birthDate), "Data de nascimento deve estar no passado.");

        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight), "Peso deve ser maior que zero.");

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Altura deve ser maior que zero.");

        return new Student(userId, birthDate, weight, height, bloodType, emergencyContact, emergencyPhone, medicalNotes);
    }

    public void UpdateBiometrics(decimal weight, decimal height)
    {
        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight), "Peso deve ser maior que zero.");

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Altura deve ser maior que zero.");

        Weight = weight;
        Height = height;
        Touch();
    }

    public void UpdateEmergencyContact(PhoneNumber? emergencyContact, PhoneNumber? emergencyPhone)
    {
        EmergencyContact = emergencyContact;
        EmergencyPhone = emergencyPhone;
        Touch();
    }

    public void UpdateMedicalInfo(string? bloodType, string? medicalNotes)
    {
        BloodType = bloodType;
        MedicalNotes = medicalNotes;
        Touch();
    }
}
