using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class CnpjTests
{
    private const string FormattedCnpj = "76.466.554/0001-40";
    private const string NormalizedCnpj = "76466554000140";

    [Fact]
    public void Create_GivenValidCnpj_ShouldCreateCnpj()
    {
        var expectedCnpj = "76466554000140";
        var actual = Cnpj.Create(expectedCnpj);
        Assert.Equal(expectedCnpj, actual.Value);
    }

    [Fact]
    public void Create_GivenFormattedCnpj_ShouldNormalizeValue()
    {
        var actual = Cnpj.Create(FormattedCnpj);
        Assert.Equal(NormalizedCnpj, actual.Value);
    }

    [Fact]
    public void Create_GivenNullCnpj_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cnpj.Create(null!));
    }

    [Fact]
    public void Create_GivenInvalidLength_ShouldThrowArgumentException()
    {
        //Missing two numbers
        Assert.Throws<ArgumentException>(() => Cnpj.Create("764665540001"));
    }

    [Fact]
    public void Create_GivenInvalidCheckDigits_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cnpj.Create("99999999999999"));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnFormattedCnpj()
    {
        var cnpj = Cnpj.Create(NormalizedCnpj);
        var formatted = cnpj.GetFormatted();
        Assert.Equal(FormattedCnpj, formatted);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnNormalizedValue()
    {
        var cnpj = Cnpj.Create(FormattedCnpj);
        var value = cnpj.ToString();
        Assert.Equal(NormalizedCnpj, value);
    }
}
