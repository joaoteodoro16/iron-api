namespace Iron.Domain.ValueObjects;

public sealed record Cnpj : IFormattableValue
{
    private static readonly int[] FirstCheckDigitWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] SecondCheckDigitWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    public string Value { get; }

    private Cnpj(string value) => Value = value;

    public static Cnpj Create(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ não pode ser vazio.", nameof(cnpj));

        var digits = new string([.. cnpj.Where(char.IsDigit)]);

        if (digits.Length != 14 || !IsValid(digits))
            throw new ArgumentException($"CNPJ '{cnpj}' é inválido.", nameof(cnpj));

        return new Cnpj(digits);
    }

    public string GetFormatted() => $"{Value[..2]}.{Value[2..5]}.{Value[5..8]}/{Value[8..12]}-{Value[12..]}";

    public override string ToString() => Value;

    private static bool IsValid(string digits)
    {
        if (digits.Distinct().Count() == 1)
            return false;

        var firstCheckDigit = CalculateCheckDigit(digits[..12], FirstCheckDigitWeights);
        var secondCheckDigit = CalculateCheckDigit(digits[..12] + firstCheckDigit, SecondCheckDigitWeights);

        return digits[12] - '0' == firstCheckDigit && digits[13] - '0' == secondCheckDigit;
    }

    private static int CalculateCheckDigit(string digits, int[] weights)
    {
        var sum = 0;
        for (var i = 0; i < digits.Length; i++)
            sum += (digits[i] - '0') * weights[i];

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
