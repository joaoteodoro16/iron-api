namespace Iron.Domain.ValueObjects;

public sealed record Cpf : IFormattableValue
{
    public string Value { get; }

    private Cpf(string value) => Value = value;

    public static Cpf Create(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));

        var digits = new string([.. cpf.Where(char.IsDigit)]);

        if (digits.Length != 11 || !IsValid(digits))
            throw new ArgumentException($"CPF '{cpf}' é inválido.", nameof(cpf));

        return new Cpf(digits);
    }

    public string GetFormatted() => $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..]}";

    public override string ToString() => Value;

    private static bool IsValid(string digits)
    {
        if (digits.Distinct().Count() == 1)
            return false;

        var firstCheckDigit = CalculateCheckDigit(digits[..9], 10);
        var secondCheckDigit = CalculateCheckDigit(digits[..9] + firstCheckDigit, 11);

        return digits[9] - '0' == firstCheckDigit && digits[10] - '0' == secondCheckDigit;
    }

    private static int CalculateCheckDigit(string digits, int firstWeight)
    {
        var sum = 0;
        var weight = firstWeight;

        foreach (var digit in digits)
            sum += (digit - '0') * weight--;

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
