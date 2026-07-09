using Iron.Domain.ValueObjects;

namespace Iron.Domain.Tests.ValueObjects;

public class EmailTests
{

    private readonly string ValidEmail = "joao@gmail.com";

    [Fact]
    public void Create_GivenValidEmail_ShouldCreateEmail()
    {
        var actual = Email.Create(ValidEmail);
        Assert.Equal(ValidEmail, actual.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_GivenEmpytOrNullEmail_ShouldThrowArgumentException(string? invalidEmail)
    {
        Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail!));
    }

    [Fact]
    public void Create_GivenInvalidEmail_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Email.Create("joao.teodoro123"));
    }

    [Fact]
    public void GetFormatted_WhenCalled_ShouldReturnValue()
    {
        var email = Email.Create(ValidEmail);
        var value = email.GetFormatted();
        Assert.Equal(ValidEmail, value);
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnValue()
    {
        var email = Email.Create(ValidEmail);
        var value = email.ToString();
        Assert.Equal(ValidEmail, value);
    }

}
