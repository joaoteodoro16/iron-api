using Bogus;
using FluentValidation.TestHelper;
using Iron.Aplication.DTOs;
using Iron.Aplication.Validators;

namespace Iron.Aplication.Tests.Validators;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator = new();

    [Fact]
    public void Validate_GivenValidRequest_ShouldBeValid()
    {
        var validator = new CreateUserRequestValidator();
        var request = new CreateUserRequest("João", "Silva", "joao@x.com", "senha1234", "14996431245", false);
        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_FirstNameEmpty_ShouldHaveError(string? firstName)
    {
        var request = ValidRequest() with { FirstName = firstName! };
        _validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Theory]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public void Validate_FirstNameLength_RespectsMaxLength(int length, bool shouldBeValid)
    {
        var request = ValidRequest() with { FirstName = new string('a', length) };
        var result = _validator.TestValidate(request);

        if (shouldBeValid) result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        else result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_GivenEmptyLastName_ShouldHaveError()
    {
        var validator = new CreateUserRequestValidator();
        var request = new CreateUserRequest("joao", "", "joao@x.com", "senha1234", "14999999999");

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_GivenEmptyEmail_ShouldHaveError()
    {
        var validator = new CreateUserRequestValidator();
        var request = new CreateUserRequest("Joao", "teodoro", "", "senha1234", "14999999999");

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_GivenEmptyPassword_ShouldHaveError()
    {
        var validator = new CreateUserRequestValidator();
        var request = new CreateUserRequest("Joao", "teodoro", "joao@gmail.com", "", "14999999999");

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_GivenEmptyPhoneNumber_ShouldHaveError()
    {
        var validator = new CreateUserRequestValidator();
        var request = new CreateUserRequest("Joao", "teodoro", "joao@gmail.com", "123123123", "");

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    // Base 100% válido — cada teste usa 'with' pra sobrescrever só o campo sob teste.
    private static CreateUserRequest ValidRequest() =>
        new("João", "Silva", "joao@x.com", "senha1234", "14996431245");
}
