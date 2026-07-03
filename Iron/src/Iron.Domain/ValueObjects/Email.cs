using System.Text.RegularExpressions;

namespace Iron.Domain.ValueObjects;

public sealed partial record Email : IFormattableValue
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio.", nameof(email));

        var normalized = email.Trim().ToLowerInvariant();

        if (!EmailRegex().IsMatch(normalized))
            throw new ArgumentException($"Email '{email}' é inválido.", nameof(email));

        return new Email(normalized);
    }

    public string GetFormatted() => Value;

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
