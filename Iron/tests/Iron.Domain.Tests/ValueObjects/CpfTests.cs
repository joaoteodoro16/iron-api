using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class CpfTests
{
    [Fact]
    public void Create_GivenValidCpf_ShoudCreateCpf()
    {
        var expectedCpf = "51590510828";
        var actual = Cpf.Create(expectedCpf);
        Assert.Equal(expectedCpf, actual.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_GivenEmptyOrNullCpf_ShoudThrowArgumentException(string? invalidCpf)
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create(invalidCpf!));
    }

    [Fact]
    public void Create_GivenInvalidCpfLenght_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create("5159051056"));
    }

    [Fact]
    public void Create_GivenInvalidCpfDigits_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cpf.Create("51590652365"));
    }

    [Fact]
    public void GetFormatted_WhemCalled_ShoudReturnMaskedCpf()
    {
        var normalizeCpf = "51590510828";
        var expectedMaskedCpf = Cpf.Create(normalizeCpf).GetFormatted();
        Assert.Equal(expectedMaskedCpf, expectedMaskedCpf);
    }

    [Fact]
    public void ToString_WhenCalled_ShoudReturnNormalizedCpfValue()
    {
        var expectedCpf = "51590510828";
        var actual = Cpf.Create(expectedCpf);
        var value = actual.ToString();
        Assert.Equal(expectedCpf, value);
    }
}
