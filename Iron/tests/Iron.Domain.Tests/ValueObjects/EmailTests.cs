using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class EmailTests
{
    private const string ValidEmail = "joao@gmail.com";

    [Fact]
    public void Create_GivenValidEmail_ShouldCreateEmail()
    {
        var actual = Email.Create(ValidEmail);
        Assert.Equal(ValidEmail, actual.Value);
    }

    [Theory]
    [InlineData("  joao@gmail.com  ")]
    [InlineData("JOAO@GMAIL.COM")]
    [InlineData(" Joao@Gmail.Com ")]
    public void Create_GivenUnnormalizedEmail_ShouldNormalizeValue(string email)
    {
        Assert.Equal(ValidEmail, Email.Create(email).Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_GivenEmptyOrNullEmail_ShouldThrowArgumentException(string? invalidEmail)
    {
        Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail!));
    }

    [Theory]
    [InlineData("joao.teodoro123")] //sem @
    [InlineData("joao@gmail")]      //domínio sem ponto
    [InlineData("@gmail.com")]      //sem a parte local
    [InlineData("joao@")]           //sem domínio
    [InlineData("joao@@gmail.com")] //dois @
    [InlineData("jo ao@gmail.com")] //espaço no meio
    public void Create_GivenInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnValue()
    {
        var value = Email.Create(ValidEmail).GetFormatted();
        Assert.Equal(ValidEmail, value);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnValue()
    {
        var value = Email.Create(ValidEmail).ToString();
        Assert.Equal(ValidEmail, value);
    }
}
