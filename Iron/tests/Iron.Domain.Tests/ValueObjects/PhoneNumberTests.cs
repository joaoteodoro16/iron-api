using AwesomeAssertions;
using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class PhoneNumberTests
{
    private const string ValidPhoneNumber = "14996431278";
    private const string FormattedPhoneNumber = "(14) 99643-1278";

    [Fact]
    public void Create_GivenValidPhoneNumber_ShouldCreatePhoneNumber()
    {
        var result = PhoneNumber.Create(ValidPhoneNumber);
        result.Value.Should().Be(ValidPhoneNumber);
    }

    [Theory]
    [InlineData("5514996431278")]
    [InlineData("(14) 99643-1278")]
    public void Create_GivenUnnormalizedPhoneNumber_ShouldNormalizeValue(string phoneNumber)
    {
        var result = PhoneNumber.Create(phoneNumber);
        result.Value.Should().Be(ValidPhoneNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_GivenEmptyOrNullPhoneNumber_ShouldThrowArgumentException(string? phoneNumber)
    {
        Assert.Throws<ArgumentException>(() => PhoneNumber.Create(phoneNumber!));
    }

    [Theory]
    [InlineData("996431254")]   //Menos de 11 dígitos
    [InlineData("26996431278")] //DDD inválido (26 não existe)
    [InlineData("14533449699")] //Telefone fixo não é permitido (3º dígito != 9)
    [InlineData("14951234567")] //4º dígito fora da faixa 6-9
    public void Create_GivenInvalidPhoneNumber_ShouldThrowArgumentException(string phoneNumber)
    {
        Assert.Throws<ArgumentException>(() => PhoneNumber.Create(phoneNumber));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnMaskedPhoneNumber()
    {
        var result = PhoneNumber.Create(ValidPhoneNumber).GetFormatted();
        result.Should().Be(FormattedPhoneNumber);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnNormalizedValue()
    {
        var result = PhoneNumber.Create(FormattedPhoneNumber).ToString();
        result.Should().Be(ValidPhoneNumber);
    }
}
