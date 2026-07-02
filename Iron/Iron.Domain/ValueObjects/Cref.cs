using System.Text.RegularExpressions;

namespace Iron.Domain.ValueObjects;

public sealed partial record Cref : IFormattableValue
{
    private static readonly HashSet<string> ValidUfs =
    [
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
        "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
        "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    ];

    public string Number { get; }
    public char Category { get; }
    public string Uf { get; }

    public string Value => $"{Number}{Category}{Uf}";

    private Cref(string number, char category, string uf)
    {
        Number = number;
        Category = category;
        Uf = uf;
    }

    public static Cref Create(string cref)
    {
        if (string.IsNullOrWhiteSpace(cref))
            throw new ArgumentException("CREF não pode ser vazio.", nameof(cref));

        var normalized = cref.Trim().ToUpperInvariant();
        var match = CrefRegex().Match(normalized);

        if (!match.Success)
            throw new ArgumentException($"CREF '{cref}' é inválido. Formato esperado: 000000-G/UF.", nameof(cref));

        var uf = match.Groups["uf"].Value;
        if (!ValidUfs.Contains(uf))
            throw new ArgumentException($"UF '{uf}' é inválida.", nameof(cref));

        return new Cref(match.Groups["number"].Value, match.Groups["category"].Value[0], uf);
    }

    public string GetFormatted() => $"{Number}-{Category}/{Uf}";

    public override string ToString() => GetFormatted();

    [GeneratedRegex(@"^(?<number>\d{6})-?(?<category>[GP])/?(?<uf>[A-Z]{2})$")]
    private static partial Regex CrefRegex();
}
