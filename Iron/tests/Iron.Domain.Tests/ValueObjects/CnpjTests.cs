using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class CnpjTests
{
    private const string FormattedCnpj = "76.466.554/0001-40";
    private const string NormalizedCnpj = "76466554000140";

    [Fact]
    public void Create_GivenValidCnpj_ShouldCreateCnpj()
    {
        var actual = Cnpj.Create(NormalizedCnpj);
        Assert.Equal(NormalizedCnpj, actual.Value);
    }

    [Fact]
    public void Create_GivenFormattedCnpj_ShouldNormalizeValue()
    {
        var actual = Cnpj.Create(FormattedCnpj);
        Assert.Equal(NormalizedCnpj, actual.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_GivenEmptyOrNullCnpj_ShouldThrowArgumentException(string? invalidCnpj)
    {
        Assert.Throws<ArgumentException>(() => Cnpj.Create(invalidCnpj!));
    }

    [Fact]
    public void Create_GivenInvalidLength_ShouldThrowArgumentException()
    {
        //Faltam dois dígitos.
        Assert.Throws<ArgumentException>(() => Cnpj.Create("764665540001"));
    }

    [Fact]
    public void Create_GivenInvalidCheckDigits_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cnpj.Create("76466554000141"));
    }

    [Fact]
    public void Create_GivenRepeatedDigits_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cnpj.Create("99999999999999"));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnFormattedCnpj()
    {
        var formatted = Cnpj.Create(NormalizedCnpj).GetFormatted();
        Assert.Equal(FormattedCnpj, formatted);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnNormalizedValue()
    {
        var value = Cnpj.Create(FormattedCnpj).ToString();
        Assert.Equal(NormalizedCnpj, value);
    }
}
