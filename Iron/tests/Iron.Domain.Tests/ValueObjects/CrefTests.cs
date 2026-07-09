using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class CrefTests
{
    private const string FormattedCref = "123456-G/SP";
    private const string NormalizedCref = "123456GSP";

    [Fact]
    public void Create_GivenValidCref_ShouldCreateCref()
    {
        var actual = Cref.Create(FormattedCref);

        Assert.Equal("123456", actual.Number);
        Assert.Equal('G', actual.Category);
        Assert.Equal("SP", actual.Uf);
        Assert.Equal(NormalizedCref, actual.Value);
    }

    [Theory]
    [InlineData("123456-G/SP")]
    [InlineData("123456GSP")]
    [InlineData("123456-GSP")]
    [InlineData("123456G/SP")]
    [InlineData("  123456-g/sp  ")]
    public void Create_GivenAcceptedFormats_ShouldNormalizeValue(string cref)
    {
        var actual = Cref.Create(cref);
        Assert.Equal(NormalizedCref, actual.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_GivenEmptyOrNullCref_ShouldThrowArgumentException(string? invalidCref)
    {
        Assert.Throws<ArgumentException>(() => Cref.Create(invalidCref!));
    }

    [Theory]
    [InlineData("12345-G/SP")]
    [InlineData("1234567-G/SP")]
    [InlineData("abcdef-G/SP")]
    [InlineData("123456-X/SP")]
    [InlineData("123456-G/S")]
    [InlineData("123456-G/SPX")]
    public void Create_GivenInvalidFormat_ShouldThrowArgumentException(string invalidCref)
    {
        Assert.Throws<ArgumentException>(() => Cref.Create(invalidCref));
    }

    [Fact]
    public void Create_GivenUnknownUf_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cref.Create("123456-G/XX"));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnFormattedCref()
    {
        var cref = Cref.Create(NormalizedCref);
        var formatted = cref.GetFormatted();
        Assert.Equal(FormattedCref, formatted);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnFormattedCref()
    {
        var cref = Cref.Create(NormalizedCref);
        var value = cref.ToString();
        Assert.Equal(FormattedCref, value);
    }
}
