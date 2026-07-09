using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class CpfTests
{
    private const string FormattedCpf = "515.905.108-28";
    private const string NormalizedCpf = "51590510828";

    [Fact]
    public void Create_GivenValidCpf_ShouldCreateCpf()
    {
        var actual = Cpf.Create(NormalizedCpf);
        Assert.Equal(NormalizedCpf, actual.Value);
    }

    [Fact]
    public void Create_GivenFormattedCpf_ShouldNormalizeValue()
    {
        var actual = Cpf.Create(FormattedCpf);
        Assert.Equal(NormalizedCpf, actual.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_GivenEmptyOrNullCpf_ShouldThrowArgumentException(string? invalidCpf)
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create(invalidCpf!));
    }

    [Fact]
    public void Create_GivenInvalidLength_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create("5159051056"));
    }

    [Fact]
    public void Create_GivenInvalidCheckDigits_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create("51590652365"));
    }

    [Fact]
    public void Create_GivenRepeatedDigits_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create("11111111111"));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnMaskedCpf()
    {
        var actual = Cpf.Create(NormalizedCpf).GetFormatted();
        Assert.Equal(FormattedCpf, actual);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnNormalizedCpfValue()
    {
        var actual = Cpf.Create(FormattedCpf).ToString();
        Assert.Equal(NormalizedCpf, actual);
    }
}
