namespace Iron.Domain.ValueObjects;

public sealed record PhoneNumber : IFormattableValue
{
    private static readonly HashSet<string> ValidDdds =
    [
        "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "21", "22", "24",
        "27", "28",
        "31", "32", "33", "34", "35", "37", "38",
        "41", "42", "43", "44", "45", "46", "47", "48", "49",
        "51", "53", "54", "55",
        "61",
        "62", "64",
        "63",
        "65", "66",
        "67",
        "68",
        "69",
        "71", "73", "74", "75", "77",
        "79",
        "81", "87",
        "82",
        "83",
        "84",
        "85", "88",
        "86", "89",
        "91", "93", "94",
        "92", "97",
        "95",
        "96",
        "98", "99"
    ];

    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Número de telefone não pode ser vazio.", nameof(phoneNumber));

        var digits = new string([.. phoneNumber.Where(char.IsDigit)]);

        if (digits.Length == 13 && digits.StartsWith("55"))
            digits = digits[2..];

        if (digits.Length != 11)
            throw new ArgumentException($"Número de telefone '{phoneNumber}' deve conter DDD + celular com 9 dígitos.", nameof(phoneNumber));

        var ddd = digits[..2];
        if (!ValidDdds.Contains(ddd))
            throw new ArgumentException($"DDD '{ddd}' é inválido.", nameof(phoneNumber));

        if (digits[2] != '9')
            throw new ArgumentException("Apenas números de celular são aceitos (número fixo não é permitido).", nameof(phoneNumber));

        if (digits[3] is < '6' or > '9')
            throw new ArgumentException($"Número de telefone '{phoneNumber}' não é um celular brasileiro válido.", nameof(phoneNumber));

        return new PhoneNumber(digits);
    }

    public string GetFormatted() => $"({Value[..2]}) {Value[2..7]}-{Value[7..]}";

    public override string ToString() => Value;
}
